﻿using FluentAssertions;
using FluentAssertions.Execution;
using JikanDotNet.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JikanDotNet.Tests
{
	public class TopTests
	{
		private readonly IJikan _jikan;

		public TopTests()
		{
			_jikan = new Jikan(true);
		}

		[Fact]
		public async Task GetAnimeTop_NoParameter_ShouldParseTopAnime()
		{
			// When
			var top = await _jikan.GetAnimeTop();

			top.Should().NotBeNull();
		}

		[Fact]
		public async Task GetAnimeTop_NoParameter_ShouldParseFMA()
		{
			// When
			var top = await _jikan.GetAnimeTop();

			// Then
			top.Top.First().Title.Should().Be("Fullmetal Alchemist: Brotherhood");
		}

		[Fact]
		public async Task GetAnimeTop_NoParameter_ShouldParseFMAEpisodes()
		{
			// When
			var top = await _jikan.GetAnimeTop();

			// Then
			top.Top.First().Episodes.Should().Be(64);
		}

		[Fact]
		public async Task GetAnimeTop_NoParameter_ShouldParseLOGHType()
		{
			// When
			var top = await _jikan.GetAnimeTop();

			// Then
			top.Top.Skip(4).First().Type.Should().Be("OVA");
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task GetAnimeTop_InvalidPage_ShouldThrowValidationException(int page)
		{
			// When
			Func<Task<AnimeTop>> func = _jikan.Awaiting(x => x.GetAnimeTop(page));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData((TopAnimeExtension)int.MaxValue)]
		[InlineData((TopAnimeExtension)int.MinValue)]
		public async Task GetAnimeTop_InvalidType_ShouldThrowValidationException(TopAnimeExtension topAnimeExtension)
		{
			// When
			Func<Task<AnimeTop>> func = this._jikan.Awaiting(x => x.GetAnimeTop(topAnimeExtension));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task GetAnimeTop_ValidTypeInvalidPage_ShouldThrowValidationException(int page)
		{
			// When
			Func<Task<AnimeTop>> func = _jikan.Awaiting(x => x.GetAnimeTop(page, TopAnimeExtension.TopAiring));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData((TopAnimeExtension)int.MaxValue)]
		[InlineData((TopAnimeExtension)int.MinValue)]
		public async Task GetAnimeTop_InvalidTypeValidPage_ShouldThrowValidationException(TopAnimeExtension topAnimeExtension)
		{
			// When
			Func<Task<AnimeTop>> func = this._jikan.Awaiting(x => x.GetAnimeTop(1, topAnimeExtension));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Fact]
		public async Task GetAnimeTop_FirstPageMovies_ShouldParseKimiNoNaWaMovieList()
		{
			// When
			var top = await _jikan.GetAnimeTop(1, TopAnimeExtension.TopMovies);

			// Then
			top.Top.First().Title.Should().Be("Kimi no Na wa.");
		}

		[Fact]
		public async Task GetAnimeTop_FirstPagePopularity_ShouldParseDeathNotePopularityList()
		{
			// When
			var top = await _jikan.GetAnimeTop(1, TopAnimeExtension.TopPopularity);

			// Then
			top.Top.First().Title.Should().Be("Death Note");
		}

		[Fact]
		public async Task GetAnimeTop_FirstPageOva_ShouldParseLOGHOvaList()
		{
			// When
			var top = await _jikan.GetAnimeTop(1, TopAnimeExtension.TopOva);

			// Then
			top.Top.First().Title.Should().Be("Ginga Eiyuu Densetsu");
		}

		[Fact]
		public async Task GetAnimeTop_FirstPageMovies_ShouldParseLOGHOAiringStartOvaList()
		{
			// When
			var top = await _jikan.GetAnimeTop(1, TopAnimeExtension.TopOva);

			// Then
			top.Top.First().AiringStart.Should().Be("Jan 1988");
		}

		[Fact]
		public async Task GetAnimeTop_SecondPage_ShouldParseAnimeSecondPage()
		{
			// When
			var top = await _jikan.GetAnimeTop(2);

			// Then
			var titles = top.Top.Select(x => x.Title);
			using (new AssertionScope())
			{
				titles.Should().Contain("Ping Pong the Animation");
				titles.Should().Contain("Yojouhan Shinwa Taikei");
			}
		}

		[Fact]
		public async Task GetMangaTop_NoParameter_ShouldParseTopManga()
		{
			// When
			var top = await _jikan.GetMangaTop();

			// Then
			top.Should().NotBeNull();
		}

		[Fact]
		public async Task GetMangaTop_NoParameter_ShouldParseBerserk()
		{
			// When
			var top = await _jikan.GetMangaTop();

			// Then
			top.Top.First().Title.Should().Be("Berserk");
		}

		[Fact]
		public async Task GetMangaTop_NoParameter_ShouldParseBerserkStartDate()
		{
			// When
			var top = await _jikan.GetMangaTop();

			// Then
			top.Top.First().PublishingStart.Should().Be("Aug 1989");
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task GetMangaTop_InvalidPage_ShouldThrowValidationException(int page)
		{
			// When
			Func<Task<MangaTop>> func = _jikan.Awaiting(x => x.GetMangaTop(page));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData((TopMangaExtension)int.MinValue)]
		[InlineData((TopMangaExtension)int.MaxValue)]
		public async Task GetMangaTop_InvalidType_ShouldThrowValidationException(TopMangaExtension topMangaExtension)
		{
			// When
			Func<Task<MangaTop>> func = _jikan.Awaiting(x => x.GetMangaTop(topMangaExtension));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task GetMangaTop_ValidTypeInvalidPage_ShouldThrowValidationException(int page)
		{
			// When
			Func<Task<MangaTop>> func = _jikan.Awaiting(x => x.GetMangaTop(page, TopMangaExtension.TopPopularity));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Theory]
		[InlineData((TopMangaExtension)int.MinValue)]
		[InlineData((TopMangaExtension)int.MaxValue)]
		public async Task GetMangaTop_InvalidTypeValidPage_ShouldThrowValidationException(TopMangaExtension topMangaExtension)
		{
			// When
			Func<Task<MangaTop>> func = _jikan.Awaiting(x => x.GetMangaTop(1, topMangaExtension));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Fact]
		public async Task GetMangaTop_FirstPageNovels_ShouldParseMonogatariTitle()
		{
			// When
			var top = await _jikan.GetMangaTop(1, TopMangaExtension.TopNovel);

			// Then
			top.Top.First().Title.Should().Be("Monogatari Series: First Season");
		}

		[Fact]
		public async Task GetMangaTop_FirstPagePopularity_ShouldParseShingekiPopularity()
		{
			// When
			var top = await _jikan.GetMangaTop(1, TopMangaExtension.TopPopularity);

			// Then
			top.Top.First().Title.Should().Be("Shingeki no Kyojin");
		}

		[Fact]
		public async Task GetMangaTop_FirstPagePopularity_ShouldParseNarutoTypePopularity()
		{
			// When
			var top = await _jikan.GetMangaTop(1, TopMangaExtension.TopPopularity);

			// Then
			top.Top.First().Type.Should().Be("Manga");
		}

		[Fact]
		public async Task GetPeopleTop_NoParameters_ShouldParseKanaHanazawa()
		{
			// When
			var top = await _jikan.GetPeopleTop();

			// Then
			var kana = top.Top.First();
			using (new AssertionScope())
			{
				kana.Name.Should().Be("Hanazawa, Kana");
				kana.NameKanji.Should().Be("花澤 香菜");
				kana.MalId.Should().Be(185);
				kana.Birthday.Value.Year.Should().Be(1989);
				kana.Favorites.Should().BeGreaterThan(60000);
			}
		}

		[Fact]
		public async Task GetPeopleTop_NoParameters_ShouldParseHiroshiKamiya()
		{
			// When
			var top = await _jikan.GetPeopleTop();

			// Then
			var kamiya = top.Top.Skip(1).First();
			using (new AssertionScope())
			{
				kamiya.Name.Should().Be("Kamiya, Hiroshi");
				kamiya.NameKanji.Should().Be("神谷 浩史");
				kamiya.MalId.Should().Be(118);
				kamiya.Birthday.Value.Year.Should().Be(1975);
				kamiya.Favorites.Should().BeGreaterThan(50000);
			}
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task GetPeopleTop_InvalidPage_ShouldThrowValidationException(int page)
		{
			// When
			Func<Task<PeopleTop>> func = _jikan.Awaiting(x => x.GetPeopleTop(page));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Fact]
		public async Task GetPeopleTop_SecondPage_ShouldFindKentarouMiura()
		{
			// When
			var top = await _jikan.GetPeopleTop(2);

			// Then
			top.Top.Select(x => x.Name).Should().Contain("Miura, Kentarou");
		}

		[Fact]
		public async Task GetCharactersTop_NoParameters_ShouldParseLelouchLamperouge()
		{
			// When
			var top = await _jikan.GetCharactersTop();

			// Then
			var lelouch = top.Top.First();
			using (new AssertionScope())
			{
				lelouch.Name.Should().Be("Lamperouge, Lelouch");
				lelouch.NameKanji.Should().Be("ルルーシュ・ランペルージ");
				lelouch.MalId.Should().Be(417);
				lelouch.Favorites.Should().BeGreaterThan(85000);
			}
		}

		[Fact]
		public async Task GetCharactersTop_NoParameters_ShouldParseLLawliet()
		{
			// When
			var top = await _jikan.GetCharactersTop();

			// Then
			var l = top.Top.Skip(1).First();
			using (new AssertionScope())
			{
				l.Name.Should().Be("Lawliet, L");
				l.NameKanji.Should().Be("エル ローライト");
				l.MalId.Should().Be(71);
				l.Favorites.Should().BeGreaterThan(65000);
			}
		}

		[Fact]
		public async Task GetCharactersTop_NoParameters_ShouldParseLuffyAnimeography()
		{
			// When
			var top = await _jikan.GetCharactersTop();

			// Then
			using (new AssertionScope())
			{
				top.Top.Skip(2).First().Name.Should().Be("Monkey D., Luffy");
				top.Top.Skip(2).First().Animeography.Select(x => x.Name).Should().Contain("One Piece");
			}
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task GetCharactersTop_InvalidPage_ShouldThrowValidationException(int page)
		{
			// When
			Func<Task<CharactersTop>> func = _jikan.Awaiting(x => x.GetCharactersTop(page));

			// Then
			await func.Should().ThrowExactlyAsync<JikanValidationException>();
		}

		[Fact]
		public async Task GetCharactersTop_SecondPage_ShouldFindTachibanaKanade()
		{
			// When
			var top = await _jikan.GetCharactersTop(2);

			// Then
			top.Top.Select(x => x.Name).Should().Contain("Tachibana, Kanade");
		}
	}
}