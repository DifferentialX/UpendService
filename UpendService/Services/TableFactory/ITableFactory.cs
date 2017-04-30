using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpendService.Models;

namespace UpendService.Services.TableFactory
{
	public interface ITableFactory
	{
		ITable CreateTable<T>() where T : Data<T>;
	}
}
