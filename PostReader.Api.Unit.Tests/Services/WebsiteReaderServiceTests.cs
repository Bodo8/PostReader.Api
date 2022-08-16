using AutoMapper;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using PostReader.Api.Application.PostWebsites.Mappings;
using PostReader.Api.Application.PostWebsites.Queries;
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
        public async void GetPosts_LessThanOneHundred_ReturnsNinetyFiveOfPosts()
        {
            //arrange 
            string posts = File.ReadAllText("..\\..\\..\\assets\\.euroJsonFirst.txt");
            string postsSecond = File.ReadAllText("..\\..\\..\\assets\\.eurJson.txt");
            string word = "test";
            string nextCursor = "*";
            var pathFirst = CreatePath(word, nextCursor);
            var pathTwo = CreatePath(word, nextCursor, 99);
            _requestWebsiteServicesMock.MakeGetRequestAsync(
                pathFirst, CancellationToken.None, true).Returns(Task.FromResult(posts)
                );
            _requestWebsiteServicesMock.MakeGetRequestAsync(
                pathTwo, CancellationToken.None, true).Returns(Task.FromResult(postsSecond)
                );
            int ExpectedNumberOfPosts = 95;

            //act
            var actual = await _sut.GetPosts(word, CancellationToken.None);

            //assert
            actual.Count.Should().Be(ExpectedNumberOfPosts);
            actual.First().Title.Should().Be("Haemorrhagic bullous pyoderma gangrenosum following COVID-19 vaccination.");
            actual.First().Author.Should().Be("Hung YT, Chung WH, Tsai TF, Chen CB.");
            actual.First().FirstPublicationDate.Should().Be(new DateTime(2022, 4, 18));
        }

        [Fact]
        public async void GetPosts_GratherThanOneHundred_ReturnsSeventyFiveOfPosts()
        {
            //arrange 
            string posts = File.ReadAllText("..\\..\\..\\assets\\.euroJsonSecond.txt");
            string postsSecond = File.ReadAllText("..\\..\\..\\assets\\.euroJsonSecondTwo.txt");
            string word = "test";
            string nextCursor = "AoIIP9H4TygzODU1MjIzNg";
            var pathFirst = CreatePath(word, "*");
            var pathTwo = CreatePath(word, nextCursor, 50);
            _requestWebsiteServicesMock.MakeGetRequestAsync(
                pathFirst, CancellationToken.None, true).Returns(Task.FromResult(posts)
                );
            _requestWebsiteServicesMock.MakeGetRequestAsync(
                pathTwo, CancellationToken.None, true).Returns(Task.FromResult(postsSecond)
                );
            int ExpectedNumberOfPosts = 75;

            //act
            var actual = await _sut.GetPosts(word, CancellationToken.None);

            //assert
            actual.Count.Should().Be(ExpectedNumberOfPosts);
            actual.First().Title.Should().Be("Living Lab Experience in Turin: Lifestyles and Exposure to Black Carbon.");
            actual.First().Author.Should().Be("Salimbene O, Boniardi L, Lingua AM, Ravina M, Zanetti M, Panepinto D.");
            actual.First().FirstPublicationDate.Should().Be(new DateTime(2022, 3, 24));
        }

        [Fact]
        public async void GetPosts_ReturnsAnyPosts()
        {
            //arrange 
            string posts = "test";
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

        private string CreatePath(string word, string nextCursor, int size = 25)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_pmcSettings.BaseUrl).Append(_pmcSettings.UrlAjaxFirst)
                .Append(word).Append(_pmcSettings.UrlAjaxCursor).Append(nextCursor)
                .Append(_pmcSettings.UrlAjaxMiddle).Append(size)
                .Append(_pmcSettings.UrlAjaxEnd);

            return builder.ToString();
        }
    }
}
