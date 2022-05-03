using System;
using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface ISendScheduler
    {
        public Task Run(
            string currentUserName, 
            string email, 
            string message, 
            string templateSubject, 
            DateTime timeToStart);
    }
}