using System;
using System.Collections.Generic;
using SQLite;

namespace MarvelFinder.Models
{
    [Table("Favorites")]
    public class MarvelComicItem
	{
        [PrimaryKey, Column("idFavorite"), AutoIncrement, NotNull] 
        public int Id { get; set; }

        [Column("title"), NotNull]
        public string Title { get; set; }

        [Ignore]
        public List<string> Creators { get; set; }

        [Column("creators")]
        public string CreatorsText { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("date")]
        public string Date { get; set; }

        [Column("series")]
        public string Series { get; set; }

        [Column("pageCount")]
        public string PageCount { get; set; }

        [Column("resourceUri")]
        public string ResourceURI { get; set; }

        [Column("ImageThumbnail")]
        public string ImageThumbnail { get; set; }

        [Ignore]
        public bool IsFavorite { get; set; }
    }
}
