﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetGame.Domain.DataConnection;
using PetGame.Domain.Entity;
using PetGame.Models;
using PetGame.Repositories.Interfaces;
using Action = PetGame.Domain.Entity.Action;

namespace PetGame.Repositories
{
    public class ActionRepository : IActionRepository
    {
        private readonly PetGameDbContext petGameDb;

        public ActionRepository(PetGameDbContext petGameDbContext)
        {
            this.petGameDb = petGameDbContext;
        }
        /// <summary>
        /// Create a new action on the database.
        /// </summary>
        /// <param name="userPet">UserPet object</param>
        /// <param name="actionType">ActionType enum</param>
        /// <returns>Object for the created action</returns>
        public async Task<Action> CreateAction(UserPet userPet, ActionTypeEnum actionType)
        {
            if (userPet == null)
            {
                Console.WriteLine($"{nameof(ActionRepository)} - CreateAction - BadRequest - Null UserPet");
                return null;
            }

            var action = new Action {ActionType = actionType, UserPet = userPet, Date = DateTimeOffset.UtcNow};

            await petGameDb.Actions.AddAsync(action);
            await petGameDb.SaveChangesAsync();

            return action;
        }
        /// <summary>
        /// Get all the actions done on a UserPet
        /// </summary>
        /// <param name="userPet">UserPet object</param>
        /// <returns>List of Action Objects</returns>

        public async Task<List<Action>> GetActionsListByUserPet(UserPet userPet)
        {
            if (userPet == null)
            {
                Console.WriteLine($"{nameof(ActionRepository)} - GetActionListByUserPet - BadRequest - Null UserPet");
                return null;
            }

            return await petGameDb.Actions.Where(a => a.UserPet == userPet).ToListAsync();
        }
    }
}
