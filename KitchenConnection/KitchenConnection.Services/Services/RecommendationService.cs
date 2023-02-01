using AutoMapper;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Data.UnitOfWork;
using KitchenConnection.DataLayer.Models.DTOs;
using KitchenConnection.DataLayer.Models.Entities;
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
            var recipeCollRecos=new List<Recipe>();

            if (userId != null && userId != Guid.Empty)
            {
                Expression<Func<RecommendationScore, bool>> expression = x => x.UserId == userId;
                var highestScoreTagRowsList = _unitOfWork.Repository<RecommendationScore>().GetByCondition(expression).OrderByDescending(x => x.Score).Take(length).ToList();

                if (highestScoreTagRowsList.Count>0)
                {                  
                    foreach(RecommendationScore recommendationScore in highestScoreTagRowsList)
                    {
                        var highestScoreTag = recommendationScore.TagId;
                        //get a random recipe with highestScoreTag tag from a different user
                        Expression<Func<Recipe, bool>> expression2 = x => x.UserId!=userId && x.Tags.Any(y => y.Id == highestScoreTag);                       
                        var randomRecipe = _unitOfWork.Repository<Recipe>().GetByCondition(expression2).OrderBy(x=> Guid.NewGuid()).FirstOrDefault();//random order by new unique Guid
                        if (randomRecipe != null && !recipeCollRecos.Contains(randomRecipe))
                        {
                            recipeCollRecos.Add(randomRecipe);
                        }                        
                    }
                    
                    return recipeCollRecos;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        public async Task<Recipe> GetSingleRecommendation(Guid? userId)
        {
            if(userId != null && userId != Guid.Empty)
            {
                Expression<Func<RecommendationScore,bool>> expression=x=>x.UserId==userId;
                var highestScoreTagRow= _unitOfWork.Repository<RecommendationScore>().GetByCondition(expression).OrderByDescending(x => x.Score).Take(1).FirstOrDefault();
                
                if(highestScoreTagRow!=null)
                {
                    var highestScoreTag = highestScoreTagRow.TagId;
                    //get a random recipe with highestScoreTag tag from a different user
                    Expression<Func<Recipe, bool>> expression2 = x =>x.UserId!=userId && x.Tags.Any(y=>y.Id==highestScoreTag);
                    return _unitOfWork.Repository<Recipe>().GetByCondition(expression2).OrderBy(x => Guid.NewGuid()).FirstOrDefault();//random order by new unique Guid
                                                                           
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> SetScore(Guid? userId, Guid? tagId)
        {
            if ((userId != null && userId != Guid.Empty) && (tagId != null && tagId != Guid.Empty))
            {
                Expression<Func<RecommendationScore, bool>> expression = x => x.UserId == userId && x.TagId == tagId;
                var existingRow = _unitOfWork.Repository<RecommendationScore>().GetByCondition(expression).FirstOrDefault();

                if (existingRow != null)
                {
                    existingRow.Score += 1;

                    _unitOfWork.Repository<RecommendationScore>().Update(existingRow);
                }
                else
                {
                    RecommendationScoreCreateDto recommendationScoreCreateDto = new RecommendationScoreCreateDto();
                    recommendationScoreCreateDto.UserId = (Guid)userId;
                    recommendationScoreCreateDto.TagId = (Guid)tagId;
                    recommendationScoreCreateDto.Score = 1;                  

                    RecommendationScore recommendationScore = _mapper.Map<RecommendationScore>(recommendationScoreCreateDto);

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
                    RecommendationScoreCreateDto recommendationScoreCreateDto = new RecommendationScoreCreateDto();
                    recommendationScoreCreateDto.UserId = (Guid)userId;
                    recommendationScoreCreateDto.TagId = (Guid)tagId;
                    recommendationScoreCreateDto.Score = 1;

                    //check if setScore was called from reviewService
                    if (score != null)
                    {
                        //1-5 stars review
                        if (score < 3)
                        {
                            recommendationScoreCreateDto.Score -= 1;
                        }
                        else if (score > 3)
                        {
                            recommendationScoreCreateDto.Score += 1;
                        }
                        //if score==3 do nothing   
                    }

                    RecommendationScore recommendationScore = _mapper.Map<RecommendationScore>(recommendationScoreCreateDto);

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
