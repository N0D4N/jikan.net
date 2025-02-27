﻿using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using JikanDotNet.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JikanDotNet.Tests
{
	public class SeasonTests
	{
		private readonly IJikan _jikan;

		public SeasonTests()
		{
			_jikan = new Jikan();
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(999)]
		[InlineData(10000)]
		[InlineData(int.MaxValue)]
		public async Task GetSeason_InvalidYear_ShouldThrowValidationException(int year)
		{
			// When
			Func<Task<Season>> func = _jikan.Awaiting(x => x.GetSeason(year, Seasons.Fall));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData((Seasons) int.MaxValue)]
		[InlineData((Seasons) int.MinValue)]
		public async Task GetSeasons_InvalidSeasonValidYear_ShouldThrowValidationException(Seasons seasons)
		{
			// When
			Func<Task<Season>> func = this._jikan.Awaiting(x=> x.GetSeason(2021,seasons));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData(1000)]
		[InlineData(1900)]
		[InlineData(2100)]
		[InlineData(9999)]
		public async Task GetSeason_ValidYearNotExistingSeason_ShouldReturnSeasonWithNulls(int year)
		{
			// When
			var season = await _jikan.GetSeason(year, Seasons.Winter);

			// Then
			using (new AssertionScope())
			{
				season.SeasonName.Should().BeNull();
				season.SeasonYear.Should().BeNull();
				season.SeasonEntries.Should().BeEmpty();
			}
		}

		[Fact]
		public async Task GetSeason_Winter2000_ShouldParseWinter2000()
		{
			// When 
			var winter2000 = await _jikan.GetSeason(2000, Seasons.Winter);

			// Then
			var titles = winter2000.SeasonEntries.Select(x => x.Title);
			using (new AssertionScope())
			{
				titles.Should().Contain("Boogiepop wa Warawanai");
				titles.Should().Contain("Ojamajo Doremi Sharp");
			}
		}

		[Fact]
		public async Task GetSeason_Spring1970_ShouldParseSpring1970()
		{
			// When 
			var spring1970 = await _jikan.GetSeason(1970, Seasons.Spring);

			// Then
			spring1970.SeasonEntries.Should().HaveCount(17);
		}

		[Fact]
		public async Task GetSeason_Winter2017_ShouldParseYoujoSenki()
		{
			// When 
			var winter2017 = await _jikan.GetSeason(2017, Seasons.Winter);

			// Then
			var youjoSenki = winter2017.SeasonEntries.FirstOrDefault(x => x.Title.Equals("Youjo Senki"));

			using (new AssertionScope())
			{
				youjoSenki.Type.Should().Be("TV");
				youjoSenki.R18.Should().BeFalse();
				youjoSenki.Kids.Should().BeFalse();
				youjoSenki.Continued.Should().BeFalse();
			}
		}

		[Fact]
		public async Task GetSeason_NoParameter_ShouldParseCurrentSeason()
		{
			// When 
			var currentSesaon = await _jikan.GetSeason();

			// Then
			using (new AssertionScope())
			{
				currentSesaon.Should().NotBeNull();
				currentSesaon.SeasonEntries.Should().HaveCountGreaterThan(20);
				currentSesaon.SeasonEntries.Should().HaveCountLessThan(500);
			}
		}

		[Fact]
		public async Task GetSeason_Spring1970_ShouldParseSpring1970ExtraInfo()
		{
			// When 
			var spring1970 = await _jikan.GetSeason(1970, Seasons.Spring);

			// Then
			using (new AssertionScope())
			{
				spring1970.SeasonName.Should().Be("Spring");
				spring1970.SeasonYear.Should().Be(1970);
			}
		}

		[Fact]
		public async Task GetSeasonArchive_NoParameter_ShouldParseFirstQueryableYear()
		{
			// When 
			var seasonArchives = await _jikan.GetSeasonArchive();

			// Then
			var oldestSeason = seasonArchives.Archives.Last();
			using (new AssertionScope())
			{
				oldestSeason.Year.Should().Be(1917);
				oldestSeason.Season.Should().HaveCount(4);
			}
		}

		[Fact]
		public async Task GetSeasonArchive_NoParameter_ShouldParseLatestQueryableYear()
		{
			// When 
			var seasonArchives = await _jikan.GetSeasonArchive();

			// Then
			using (new AssertionScope())
			{
				seasonArchives.Archives.First().Year.Should().BeGreaterOrEqualTo(DateTime.UtcNow.Year);
				seasonArchives.Archives.Last().Season.Should().HaveCountGreaterOrEqualTo(1);
				seasonArchives.Archives.Last().Season.Should().HaveCountLessOrEqualTo(4);
			}
		}

		[Fact]
		public async Task GetSeasonLater_NoParameter_ShouldParseLaterSeason()
		{
			// When 
			var season = await _jikan.GetSeasonLater();

			// Then
			using (new AssertionScope())
			{
				season.SeasonYear.Should().BeNull();
				season.SeasonName.Should().Be("Later");
			}
		}
	}
}