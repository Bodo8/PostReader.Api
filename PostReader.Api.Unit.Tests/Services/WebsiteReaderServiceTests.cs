using AutoMapper;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using PostReader.Api.Application.PostWebsites.Mappings;
using PostReader.Api.Common.CommonModels.Settings;
using PostReader.Api.Common.Interfaces;
using PostReader.Api.Infrastructure.Services.JsonModels;
using PostReader.Api.Services;
using PostReader.Api.Unit.Tests.MotherObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PostReader.Api.Unit.Tests.Services
{
    public class WebsiteReaderServiceTests
    {
        private readonly IMapper _mapper;
        private readonly WebsiteReaderService _sut;
        private readonly EuropePmcSettings _pmcSettings;
        private readonly IRequestWebsiteService _requestWebsiteServicesMock;

        public WebsiteReaderServiceTests()
        {
            MapperConfiguration mappingConfig = CreateMapConfig();
            _mapper = mappingConfig.CreateMapper();
            _pmcSettings = ObjectsToTests.GetSettings();
            _requestWebsiteServicesMock = Substitute.For<IRequestWebsiteService>();
            _sut = new WebsiteReaderService(_requestWebsiteServicesMock, _mapper, _pmcSettings);
        }

        [Fact]
        public async void GetPosts_ReturnsExpectedNumberOfPosts()
        {
            //arrange 
            string posts = File.ReadAllText("..\\..\\..\\assets\\.eurJson.txt");
            _requestWebsiteServicesMock.MakeGetRequestAsync(
                Arg.Any<string>(), CancellationToken.None, Arg.Any<bool>()).Returns(Task.FromResult(posts)
                );
            string sentence = "test";
            int ExpectedNumberOfPosts = 95;

            //act
            var actual = await _sut.GetPosts(sentence, CancellationToken.None);

            //assert
            actual.Count.Should().Be(ExpectedNumberOfPosts);
            actual.First().Title.Should().Be("Haemorrhagic bullous pyoderma gangrenosum following COVID-19 vaccination.");
            actual.First().Author.Should().Be("Hung YT, Chung WH, Tsai TF, Chen CB.");
            actual.First().FirstPublicationDate.Should().Be(new DateTime(2022, 4, 18));
        }

        [Fact]
        public async void GetPosts_ReturnsAnyPosts()
        {
            //arrange 
            string posts = "tekst";
            _requestWebsiteServicesMock.MakeGetRequestAsync(
                Arg.Any<string>(), CancellationToken.None, Arg.Any<bool>()).Returns(Task.FromResult(posts)
                );
            string sentence = "test";
            int ExpectedNumberOfPosts = 0;

            //act
            var actual = await _sut.GetPosts(sentence, CancellationToken.None);

            //assert
            actual.Count.Should().Be(ExpectedNumberOfPosts);
        }

        private static MapperConfiguration CreateMapConfig()
        {
            return new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PostsWebsiteProfile());
            });
        }
    }
}
