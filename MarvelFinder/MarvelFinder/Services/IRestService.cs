using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarvelFinder.Models;

namespace MarvelFinder.Services
{
	public interface IRestService
	{
		Task<List<MarvelComicItem>> GetComicList(string searchValue = "");
    }
}
