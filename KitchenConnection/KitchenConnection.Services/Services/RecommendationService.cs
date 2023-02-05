using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecommendationExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.Models.DTOs;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services
{
    public class RecommendationsService : IRecommendationsService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        public RecommendationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Recipe>> GetCollectionRecommendations(Guid? userId, int length)
        {
            var recipeCollectionRecommendation = new List<Recipe>();


            if (userId is null || userId == Guid.Empty)
            {
                throw new RecommendationNotFoundException("Could not make a recommendation! User not found!");
            }

            var highestScoreTagRowsList = _unitOfWork.Repository<RecommendationScore>().GetByCondition(user => user.UserId == userId).OrderByDescending(x => x.Score).Take(length).ToList();

            if(highestScoreTagRowsList is null || highestScoreTagRowsList.Count == 0)
            {
                throw new RecommendationCollectionNotFound($"Highest Tag Scores not found for User {userId}");
            }

            foreach (RecommendationScore recommendationScore in highestScoreTagRowsList)
            {
                var highestScoreTag = recommendationScore.TagId;
                //get a random recipe with highestScoreTag tag from a different user
                var randomRecipe = _unitOfWork.Repository<Recipe>()
                    .GetByCondition(x => x.UserId != userId && x.Tags.Any(y => y.Id == highestScoreTag))
                    .OrderBy(x => Guid.NewGuid()).FirstOrDefault();//random order by new unique Guid
                if (randomRecipe != null && !recipeCollectionRecommendation.Contains(randomRecipe))
                {
                    recipeCollectionRecommendation.Add(randomRecipe);
                }
            }

            if(recipeCollectionRecommendation is null || recipeCollectionRecommendation.Count == 0) throw new RecommendationCollectionNotFound($"Could not make a proper Collection Recommendation for User {userId}");

            return recipeCollectionRecommendation;
        }

        public async Task<Recipe> GetSingleRecommendation(Guid? userId)
        {
            if(userId is null || userId == Guid.Empty)
            {
                throw new RecommendationNotFoundException("Could not make a recommendation! User not found!");
            }

            var highestScoreTagRow = _unitOfWork.Repository<RecommendationScore>().GetByCondition(user => user.UserId == userId).OrderByDescending(x => x.Score).Take(1).FirstOrDefault();

            if (highestScoreTagRow is null) throw new RecommendationNotFoundException($"Highest Tag Score not found for User: {userId}");

            //get a random recipe with highestScoreTag tag from a different user
            var recommendation = _unitOfWork.Repository<Recipe>()
                .GetByCondition(x => x.UserId != userId && x.Tags.Any(y => y.Id == highestScoreTagRow.TagId))
                .OrderBy(x => Guid.NewGuid()).FirstOrDefault(); //random order by new unique Guid

            if (recommendation is null) throw new RecommendationNotFoundException($"Could not make a proper Recommendation for User: {userId} with the Tag: {highestScoreTagRow.TagId}");

            return recommendation;
        }

        public async Task<bool> SetScore(Guid? userId, Guid? tagId)
        {
            if ((userId is not null && userId != Guid.Empty) && (tagId is not null && tagId != Guid.Empty))
            {
                var existingRow = _unitOfWork.Repository<RecommendationScore>().GetByCondition(row => row.UserId == userId && row.TagId == tagId).FirstOrDefault();

                if (existingRow is not null)
                {
                    existingRow.Score += 1;

                    _unitOfWork.Repository<RecommendationScore>().Update(existingRow);
                }
                else
                {
                    RecommendationScore recommendationScore = new RecommendationScore();
                    recommendationScore.UserId = (Guid)userId;
                    recommendationScore.TagId = (Guid)tagId;
                    recommendationScore.Score = 1;

                    await _unitOfWork.Repository<RecommendationScore>().Create(recommendationScore);
                }
                _unitOfWork.Complete();
                
                return true;

            }
            else
            {
                return false;
            }
        }

        //overloaded
        //when score is sent --is called when a review is done
        public async Task<bool> SetScore(Guid? userId, Guid? tagId, int? score)
        {
            if((userId!=null && userId!=Guid.Empty) && (tagId != null && tagId!=Guid.Empty))
            {
                Expression<Func<RecommendationScore,bool>> expression=x=>x.UserId==userId && x.TagId==tagId;
                var existingRow = _unitOfWork.Repository<RecommendationScore>().GetByCondition(expression).FirstOrDefault();

                if (existingRow != null)
                {
                    if (score == null)
                    {
                        existingRow.Score += 1;
                    }
                    else
                    {
                        //1-5 stars review
                        if (score < 3)
                        {
                            existingRow.Score -=1;
                        }
                        else if(score>3)
                        {
                            existingRow.Score += 1;
                        }
                        //if score==3 do nothing                        
                    }
                    
                    _unitOfWork.Repository<RecommendationScore>().Update(existingRow);                   
                }
                else
                {
                    RecommendationScore recommendationScore = new RecommendationScore();
                    recommendationScore.UserId = (Guid)userId;
                    recommendationScore.TagId = (Guid)tagId;
                    recommendationScore.Score = 1;

                    //check if setScore was called from reviewService
                    if (score != null)
                    {
                        //1-5 stars review
                        if (score < 3)
                        {
                            recommendationScore.Score -= 1;
                        }
                        else if (score > 3)
                        {
                            recommendationScore.Score += 1;
                        }
                        //if score==3 do nothing   
                    }

                    await _unitOfWork.Repository<RecommendationScore>().Create(recommendationScore);                  
                }
                var res = _unitOfWork.Complete();               
                return true;

            }
            else
            {
                return false;
            }
        }
    }
}
