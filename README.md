 [![Discord Server](https://img.shields.io/discord/460491088004907029.svg?style=flat&logo=discord)](https://discord.gg/4tvCr36) ![build status](https://travis-ci.org/Ervie/jikan.net.svg?branch=master) ![build status](https://img.shields.io/nuget/v/JikanDotNet.svg) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT) [![GitHub issues open](https://img.shields.io/github/issues/Ervie/jikan.net.svg?maxAge=2592000)]() 

# jikan.net

Jikan.net is a .NET wrapper for [Jikan](https://jikan.moe) RESTful API for parsing data from [MyAnimeList](https://myanimelist.com). Main objective of the wrapper is to simplify utilization of Jikan API, as strongly typed languages are not-so-easy to use with elastic json (sure we can go use dynamics in .NET, but let's think about performance).

### Main attributes

* Written in to work with .Net Standard 2.0, compatible with .Net Framework (4.6.1 or newer) and .Net Core (2.0 or newer).
* Fully asynchromous request fetching (can be forced to synchromous if needed).
* Can handle both SSL encrypted and non-SSL encrypted requests.
* Light on dependencies 
    * No dependencies if you are using .Net Core 3.x.
    * Single dependancy for .Net Framework and .Net Core 2.x (System.Text.Json).
* Usable with Dependency Injection.

# List of features

- Anime
    - Basic information
    - Characters & Staff
    - Episode
    - News
    - Videos/PV/Episodes
    - Pictures
    - Stats
    - Forum Topics
    - More Info
    - Reviews
    - Recommendations
    - User Updates
- Manga
    - Basic information
    - Characters 
    - News
    - Pictures
    - Stats
    - Forum Topics
    - More Info
    - Reviews
    - Recommendations
    - User Updates
- People
    - Basic information
    - Pictures
- Characters
    - Basic information
    - Pictures
- Search (Anime/Manga/Character/Person)
    - Basic query
    - Filters (Advanced Search)
    - Pagination Support
    - No.# of pages
- Seasonal Anime 
    - Season + Year
    - Undefined airing date
- Season Archive
- Anime Scheduling (for current season)
    - Filtering by day of the week.
- Top
    - Anime
    - Manga
    - People
    - Characters
    - Sub Types & Pagination Support
- Genre
    - Anime genres
    - Manga genres
- Producer
- Magazine
- User
    - Profile
    - Friends
    - History
        - Filter by Anime/Manga.
    - Anime list
        - Filter by status (watching, completed, etc.)
        - Advanced filters
        - Pagination support
    - Manga list
        - Filter by status (reading, completed, etc.)
        - Advanced filters
        - Pagination support
- Clubs
    - Profile
    - Member list
        - Pagination support
- Meta
    - API status
# Installation

### Package manager

```
PM> Install-Package JikanDotNet
```

### .NET CLI

```
>dotnet add package JikanDotNet
```

Then restore dependencies:
```
>dotnet restore
```

# Changelog

## Future - Version 2.0.0

- Compatible with Jikan REST API v4.0

## 23.06.2021 - Version 1.5.6 (newest)

- Features
    - <b>[Validation]</b> Validation of input parameters of Enum types parameters most method are added for fail fast approach.
- Fixes
    - <b>[SearchManga]</b> Remove redundant page part during url building.

**[Read More](https://github.com/Ervie/jikan.net/blob/master/Changelog.md)**

# Documentation &  Usage example

See [project wiki](https://github.com/Ervie/jikan.net/wiki#usage-example).
