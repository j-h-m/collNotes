using collNotes.Domain.Models;
using collNotes.Ef.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace collNotes.UnitTests.Data.Context
{
    public class CollNotesContext_InMemory_Mock
    {
        public CollNotesContext GetCollNotesContext_InMemory()
        {
            return new CollNotesContext(new DbContextOptionsBuilder<CollNotesContext>()
                .UseInMemoryDatabase(databaseName: "testing_db")
                .Options);
        }

        public CollNotesContext GetCollNotesContext_InMemory_WithTestData()
        {
            var context = GetCollNotesContext_InMemory();
            List<Trip> trips = new List<Trip>()
            {
                new Trip()
                {
                    TripName = "Trip-1",
                    TripNumber = 1,
                    CollectionDate = DateTime.Now,
                    PrimaryCollector = "Tester 1",
                    AdditionalCollectors = "Tester 2, Tester 3"
                }
            };
            List<Site> sites = new List<Site>()
            {
                new Site()
                {
                    SiteName = "Site-1",
                    SiteNumber = 1,
                    AssociatedTripName = "Trip-1",
                    AssociatedTaxa = "Pinus strobus, Pinus resinosa",
                    Longitude = "34.839659",
                    Latitude = "-85.482653",
                    Locality = "near trail head"
                },
                new Site()
                {
                    SiteName = "Site-2",
                    SiteNumber = 1,
                    AssociatedTripName = "Trip-1",
                    AssociatedTaxa = "Pinus strobus, Pinus resinosa",
                    Longitude = "34.839659",
                    Latitude = "-85.482653",
                    Locality = "near trail head"
                },
                new Site()
                {
                    SiteName = "Site-3",
                    SiteNumber = 1,
                    AssociatedTripName = "Trip-1",
                    AssociatedTaxa = "Pinus strobus, Pinus resinosa",
                    Longitude = "34.839659",
                    Latitude = "-85.482653",
                    Locality = "near trail head"
                }
            };
            List<Specimen> specimen = new List<Specimen>()
            {
                new Specimen()
                {
                    SpecimenName = "1-1",
                    SpecimenNumber = 1,
                    AssociatedSiteName = "Site-1",
                    AssociatedSiteNumber = 1,
                    FieldIdentification = "Pinus virginiana",
                    IndividualCount = "100"
                },
                new Specimen()
                {
                    SpecimenName = "1-2",
                    SpecimenNumber = 2,
                    AssociatedSiteName = "Site-1",
                    AssociatedSiteNumber = 1,
                    FieldIdentification = "Pinus contorta",
                    IndividualCount = "100"
                },
                new Specimen()
                {
                    SpecimenName = "1-2",
                    SpecimenNumber = 2,
                    AssociatedSiteName = "Site-1",
                    AssociatedSiteNumber = 1,
                    FieldIdentification = "Pinus ponderosa",
                    IndividualCount = "100"
                },
                new Specimen()
                {
                    SpecimenName = "2-3",
                    SpecimenNumber = 3,
                    AssociatedSiteName = "Site-2",
                    AssociatedSiteNumber = 2,
                    FieldIdentification = "Pinus echinata",
                    IndividualCount = "100"
                },
                new Specimen()
                {
                    SpecimenName = "3-5",
                    SpecimenNumber = 5,
                    AssociatedSiteName = "Site-3",
                    AssociatedSiteNumber = 3,
                    FieldIdentification = "Pinus banksiana",
                    IndividualCount = "100"
                },
                new Specimen()
                {
                    SpecimenName = "3-6",
                    SpecimenNumber = 6,
                    AssociatedSiteName = "Site-3",
                    AssociatedSiteNumber = 3,
                    FieldIdentification = "Pinus rigida",
                    IndividualCount = "100"
                }
            };
            context.Trips.AddRange(trips);
            context.Sites.AddRange(sites);
            context.Specimen.AddRange(specimen);

            context.SaveChanges();
            return context;
        }
    }
}
