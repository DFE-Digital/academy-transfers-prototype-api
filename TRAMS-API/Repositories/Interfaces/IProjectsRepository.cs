﻿using API.Models.D365;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories.Interfaces
{
    public interface IProjectsRepository
    {
        public Task<List<GetAcademyTransfersProjectsD365Model>> GetAll();

        public Task<GetAcademyTransfersProjectsD365Model> GetProjectById(Guid id);

        public Task<Guid?> InsertProject(PostAcademyTransfersProjectsD365Model project);
    }
}
