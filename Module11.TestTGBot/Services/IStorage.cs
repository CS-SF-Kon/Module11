using Module11.TestTGBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module11.TestTGBot.Services;

public interface IStorage
{
    Session GetSession(long ChatID);
}
