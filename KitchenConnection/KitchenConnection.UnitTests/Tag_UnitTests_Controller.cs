using AutoMapper;
using KitchenConnection.BusinessLogic.Helpers;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Controllers;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.RecipeTag;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace KitchenConnection.UnitTesting
{
    public class Tag_UnitTests_Controller
    {
        private readonly Mock<ITagService> _tagMockService;
        private readonly IMapper _mapper;
        public Guid tagId1 = Guid.NewGuid();
        private Guid tagId2 = Guid.NewGuid();
        private Guid tagId3 = Guid.NewGuid();
        private User user1 = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "UserName1",
            LastName = "User Surname1",
            Gender = "Male",
            Email = "randomEmail@gjirafa.com",
            PhoneNumber = "44123123",
            DateOfBirth = new DateTime(),
            Recipes = new List<Recipe>(),
            Reviews = new List<Review>()
        };

        public Tag_UnitTests_Controller()
        {
            var mapperConfiguration = new MapperConfiguration(
             mc => mc.AddProfile(new AutoMapperConfigurations()));

            _mapper = mapperConfiguration.CreateMapper();
            _tagMockService = new Mock<ITagService>();
        }

        [Fact]
        public async void CreateTag_ShouldReturnTag()
        {
            var tags = GetMockTagsDtos();
            var tagToCreate = _mapper.Map<TagCreateDTO>(tags[0]);

            _tagMockService.Setup(x => x.Create(tagToCreate).Result)
               .Returns(tags[0]);
            var tagController = new TagsController(_tagMockService.Object);          

            ActionResult<TagDTO> actionResult = await tagController.Create(tagToCreate);

            TagDTO result = (TagDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(tags[0].Id, value);
            Assert.True(tags[0].Id == value);
        }
        [Fact]
        public async void GetTagById_ShouldReturnTag()
        {
            //arrange
            var tags = GetMockTagsDtos();

            _tagMockService.Setup(x => x.Get(tags[0].Id).Result)
               .Returns(tags[0]);
            var tagController = new TagsController(_tagMockService.Object);

            //act
            ActionResult<TagDTO> actionResult = await tagController.Get(tagId1);

            TagDTO result = (TagDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(tags[0].Id, value);
            Assert.True(tags[0].Id == value);

        }

        [Fact]
        public async void GetAllTags_ShouldReturnTags()
        {
            //arrange
            var tags = GetMockTagsDtos();

            _tagMockService.Setup(x => x.GetAll().Result)
               .Returns(tags);
            var tagController = new TagsController(_tagMockService.Object);

            //act
            ActionResult<List<TagDTO>> actionResult = await tagController.GetAll();

            List<TagDTO> result = (List<TagDTO>)(actionResult.Result as OkObjectResult).Value;

            //assert           
            Assert.NotNull(result);
            Assert.Equal(result.Count, result.Count);
            Assert.True(tags.Equals(result));
        }

        [Fact]
        public async void DeleteTagById_ShouldReturnDeletedTag()
        {
            //arrange
            var tags = GetMockTagsDtos();

            _tagMockService.Setup(x => x.Delete(tags[0].Id).Result)
                .Returns(tags[0]);
            var tagController = new TagsController(_tagMockService.Object);

            //act
            ActionResult<TagDTO> actionResult = await tagController.Delete(tagId1);

            TagDTO result = (TagDTO)(actionResult.Result as OkObjectResult).Value;

            //assert            
            var value = result.Id;

            Assert.NotNull(result);
            Assert.Equal(tags[0].Id, value);
            Assert.True(tags[0].Id == value);
        }

        [Fact]
        public async void UpdateTag_ShouldReturnUpdatedTag()
        {
            //arrange
            var tags = GetMockTagsDtos();
            var tagToUpdate = new TagDTO
            {
                Id = tags[0].Id,
                Name = tags[0].Name               
            };
                   
            tagToUpdate.Name = "Updated Tag Name";
            _tagMockService.Setup(x => x.Update(tagToUpdate).Result)
                .Returns(tagToUpdate);
            var tagController = new TagsController(_tagMockService.Object);

            //act
            ActionResult<TagDTO> actionResult = await tagController.Update(tagToUpdate);

            TagDTO result = (TagDTO)(actionResult.Result as OkObjectResult).Value;

            //assert                      
            Assert.NotNull(result);
            Assert.Equal(tags[0].Id, result.Id);
            Assert.True(tags[0].Id == result.Id);
            Assert.NotEqual(tags[0].Name, result.Name);
            Assert.False(tags[0].Name == (result.Name));
        }

        //mock data for tags
        private List<Tag> GetMockTags()
        {
            List<Tag> tags = new List<Tag>
            {
                new Tag
                {
                    Id=tagId1,
                    Name="Sweets",
                    Recipes=new List<Recipe>()
                },
                new Tag
                {
                    Id=tagId2,
                    Name="Italian",
                    Recipes=new List<Recipe>()
                },
                new Tag
                {
                    Id=tagId3,
                    Name="Albanian",
                    Recipes=new List<Recipe>()
                }
            };

            return tags;
        }

        //map tags to tagsDTOs
        public List<TagDTO> GetMockTagsDtos()
        {
            List<TagDTO> tagDtos = new List<TagDTO>();
            List<Tag> tags = GetMockTags();

            foreach (Tag tag in tags)
            {
                TagDTO t = _mapper.Map<TagDTO>(tag);
                tagDtos.Add(t);
            }

            return tagDtos;
        }



    }
}
