﻿using FilmCatalogue.Domain.Contexts.Film.Models;
using MediatR;
using System;

namespace FilmCatalogue.Domain.Repositories.Film.Commands
{
    public class AddFilm : IRequest<FilmModel>
    {
        public string Name { get; set; }
        public DateTime ShowedDate { get; set; }
        public DateTime AddedAt { get; set; }
    }
}