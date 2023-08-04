using System;
using System.Collections.Generic;
using System.Diagnostics;
using MarvelFinder.Models;

namespace MarvelFinder.Mapper
{
	internal static class MarvelComicItemMapper
	{
		public static MarvelComicItem MapToModel(this Models.MarvelComicItemDTO comic)
		{
			MarvelComicItem newComic = null;

			List<string> creators = null;
			string creatorsText = "";

            try
			{
				if(comic.creators.items.Count > 0)
				{
					creators = new List<string>();
                    comic.creators.items.ForEach(item => creators.Add($"{item.name} / {item.role}"));
					creatorsText = string.Join(", ", creators.ToArray());
                }

				newComic = new MarvelComicItem();

				newComic.Title = comic.title;
				newComic.Description = comic.description;
				newComic.Creators = creators;
				newComic.CreatorsText = creatorsText == null ? "" : creatorsText;
				newComic.Date = comic.dates[comic.dates.Count-1].date;
				newComic.Series = comic.series.name;
				newComic.PageCount = comic.pageCount;
				newComic.ResourceURI = comic.resourceURI;
				newComic.ImageThumbnail = comic.thumbnail.path + "." + comic.thumbnail.extension;
				newComic.IsFavorite = false;
			}
			catch(Exception ex)
			{
				Debug.WriteLine($"Mapper failed with error -> {ex}");
			}

			return newComic;
		}
	}
}
