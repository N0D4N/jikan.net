﻿using FluentAssertions;
using FluentAssertions.Execution;
using JikanDotNet.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JikanDotNet.Tests
{
	public class UserMangaListTests
	{
		private readonly IJikan _jikan;

		public UserMangaListTests()
		{
			_jikan = new Jikan();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("\n\n\t    \t")]
		public async Task GetUserMangaList_InvalidUsername_ShouldThrowValidationException(string username)
		{
			// When
			Func<Task<UserMangaList>> func = _jikan.Awaiting(x => x.GetUserMangaList(username));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Fact]
		public async Task GetUserMangaList_Ervelan_ShouldParseErvelanMangaList()
		{
			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan");

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Count.Should().BeGreaterThan(90);
				mangaList.Manga.Select(x => x.Title).Should().Contain("Dr. Stone");
			}
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("\n\n\t    \t")]
		public async Task GetUserMangaList_InvalidUsernameWithExtension_ShouldThrowValidationException(string username)
		{
			// When
			Func<Task<UserMangaList>> func = _jikan.Awaiting(x => x.GetUserMangaList(username, UserMangaListExtension.Reading));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData((UserMangaListExtension)int.MaxValue)]
		[InlineData((UserMangaListExtension)int.MinValue)]
		public async Task GetUserMangaList_ErvelanWithInvalidExtension_ShouldThrowValidationException(UserMangaListExtension userMangaListExtension)
		{
			// When
			Func<Task<UserMangaList>> func = _jikan.Awaiting(x => x.GetUserMangaList("Ervelan", userMangaListExtension));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData((UserMangaListExtension)int.MaxValue)]
		[InlineData((UserMangaListExtension)int.MinValue)]
		public async Task GetUserMangaList_ErvelanWithValidPageWithInvalidExtension_ShouldThrowValidationException(UserMangaListExtension userMangaListExtension)
		{
			// When
			Func<Task<UserMangaList>> func = _jikan.Awaiting(x => x.GetUserMangaList("Ervelan", userMangaListExtension, 1));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanReading_ShouldParseErvelanMangaReadingList()
		{
			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", UserMangaListExtension.Reading);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Select(x => x.Title).Should().Contain("One Piece");
				mangaList.Manga.First().ReadingStatus.Should().Be(UserMangaListExtension.Reading);
				mangaList.Manga.First().PublishingStatus.Should().Be(AiringStatus.Airing);
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanCompleted_ShouldParseErvelanMangaCompletedList()
		{
			// When
			var mangalist = await this._jikan.GetUserMangaList("Ervelan", UserMangaListExtension.Completed, 1);

			// Then
			using (new AssertionScope())
			{
				mangalist.Should().NotBeNull();
				mangalist.Manga.First().ReadingStatus.Should().Be(UserMangaListExtension.Completed);
				mangalist.Manga.First().PublishingStatus.Should().Be(AiringStatus.Completed);
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanDropped_ShouldParseErvelanMangaDroppedList()
		{
			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", UserMangaListExtension.Dropped);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Should().HaveCount(3);
				mangaList.Manga.Select(x => x.Title).Should().Contain("D.Gray-man");
			}
		}

		[Fact]
		public void GetUserMangaList_onrix_ShouldParseOnrixMangaList()
		{
			// When
			var action = _jikan.Awaiting(x => x.GetUserMangaList("onrix"));

			// Then
			action.Should().ThrowAsync<JikanRequestException>();
		}

		[Fact]
		public async Task GetUserMangaList_Mithogawa_ShouldParseMithogawaMangaList()
		{
			// When
			var mangaList = await _jikan.GetUserMangaList("Mithogawa");

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Should().HaveCount(300);
			}
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("\n\n\t    \t")]
		public async Task GetUserMangaList_InvalidUsernameWithConfig_ShouldThrowValidationException(string username)
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				Query = "death"
			};

			// When
			Func<Task<UserMangaList>> func = _jikan.Awaiting(x => x.GetUserMangaList(username, searchConfig));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData((UserListMangaPublishingStatus)int.MaxValue, null, null, null)]
		[InlineData((UserListMangaPublishingStatus)int.MinValue, null, null, null)]
		[InlineData(null, (UserListMangaSearchSortable)int.MaxValue, null, null)]
		[InlineData(null, (UserListMangaSearchSortable)int.MinValue, null, null)]
		[InlineData(null, UserListMangaSearchSortable.Priority, (UserListMangaSearchSortable)int.MaxValue, null)]
		[InlineData(null, UserListMangaSearchSortable.Priority, (UserListMangaSearchSortable)int.MinValue, null)]
		[InlineData(null, UserListMangaSearchSortable.Priority, null, (SortDirection)int.MaxValue)]
		[InlineData(null, UserListMangaSearchSortable.Priority, null, (SortDirection)int.MinValue)]
		public async Task GetUserMangaList_ErvelanWithConfigWithInvalidEnums_ShouldThrowValidationException(
			UserListMangaPublishingStatus? publishingStatus,
			UserListMangaSearchSortable? orderBy,
			UserListMangaSearchSortable? orderBy2,
			SortDirection? sortDirection
		)
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				PublishingStatus = publishingStatus.GetValueOrDefault(),
				OrderBy = orderBy.GetValueOrDefault(),
				OrderBy2 = orderBy2.GetValueOrDefault(),
				SortDirection = sortDirection.GetValueOrDefault()
			};

			// When
			Func<Task<UserMangaList>> func = _jikan.Awaiting(x => x.GetUserMangaList("Ervelan", searchConfig));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Fact]
		public async Task GetUserMangaList_ValidUsernameNullConfig_ShouldThrowValidationException()
		{
			// When
			Func<Task<UserMangaList>> func = _jikan.Awaiting(x => x.GetUserMangaList("Ervelan", null));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanFilterWithDeath_ShouldFindBothDeathNotes()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				Query = "death"
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			var titles = mangaList.Manga.Select(x => x.Title);
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				titles.Should().Contain("Death Note");
				titles.Should().Contain("Death Note Another Note: Los Angeles BB Renzoku Satsujin Jiken");
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanFilterWithStone_ShouldFindDrStoneAndJoJo()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				Query = "stone"
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			var titles = mangaList.Manga.Select(x => x.Title);
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				titles.Should().Contain("Dr. Stone");
				titles.Should().Contain("JoJo no Kimyou na Bouken Part 6: Stone Ocean");
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanFilterWithHunter_ShouldFindSingleResult()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				Query = "hunter"
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Should().ContainSingle();
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanFilterWithQwerty_ShouldNotFindResults()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				Query = "qwerty"
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Should().BeEmpty();
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanFilterWithEmptyString_ShouldReturnAllResults()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				Query = ""
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Count.Should().BeGreaterThan(90);
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanSortByTitle_ShouldFindYuuyuuHakusho()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				OrderBy = UserListMangaSearchSortable.Title,
				SortDirection = SortDirection.Ascending
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.First().Title.Should().Be("Yuu☆Yuu☆Hakusho");
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanSortByScore_ShouldFindBerserk()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				OrderBy = UserListMangaSearchSortable.Score,
				SortDirection = SortDirection.Descending
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.First().Title.Should().Be("Berserk");
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanSortByScoreThenTitle_ShouldFindFMAOn4thPlace()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				OrderBy = UserListMangaSearchSortable.Score,
				OrderBy2 = UserListMangaSearchSortable.Title,
				SortDirection = SortDirection.Descending
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.First().Title.Should().Be("Berserk");
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanSortYoungAnimal_ShouldFindBerserk()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				MagazineId = 2
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Should().ContainSingle();
				mangaList.Manga.First().Title.Should().Be("Berserk");
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanSortByWrongMagazineId_ShouldReturnAllResults()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				MagazineId = -1,
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				mangaList.Manga.Count.Should().BeGreaterThan(90);
			}
		}

		[Fact]
		public async Task GetUserMangaList_ErvelanSortByOneWithPublishingStatus_ShouldFindOPMAndOPNotCrossover()
		{
			// Given
			var searchConfig = new UserListMangaSearchConfig()
			{
				Query = "one",
				PublishingStatus = UserListMangaPublishingStatus.Publishing
			};

			// When
			var mangaList = await _jikan.GetUserMangaList("Ervelan", searchConfig);

			// Then
			var titles = mangaList.Manga.Select(x => x.Title);
			using (new AssertionScope())
			{
				mangaList.Should().NotBeNull();
				titles.Should().Contain("One Piece");
				titles.Should().Contain("One Punch-Man");
				titles.Should().NotContain("One Piece x Toriko");
			}
		}
	}
}