﻿using System;
using System.IO;
using Newtonsoft.Json;
using VotRomania.Models;

namespace VotRomania.Providers
{
    public interface IDataProvider
    {
        PollingStationsInfo[] LoadPollingStationsInfos();
        StaticData[] LoadStaticData();
    }

    public class DummyDataProvider : IDataProvider
    {
        private PollingStationsInfo[] _cachedData;
        public DummyDataProvider()
        {
        }

        public PollingStationsInfo[] LoadPollingStationsInfos()
        {
            if (_cachedData != null)
            {
                return _cachedData;
            }

            using var file = File.OpenText(@$"DummyData{Path.DirectorySeparatorChar}polling-stations.json");
            var serializer = new JsonSerializer();
            _cachedData = (PollingStationsInfo[])serializer.Deserialize(file, typeof(PollingStationsInfo[]));
            return _cachedData;
        }

        private VotingGuide GetGuide(Language language)
        {
            var prefix = language == Language.Ro ? "" : "[HU]";
            var options = new[]
            {
                new Option
                {
                    Title = $"{prefix}Cetățean cu domiciul în orașul în care doresc să votez",
                    Description =
                        $"{prefix}Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Option
                {
                    Title = $"{prefix}Cetățean cu viză de flotant în orașul în care doresc să votez",
                    Description =
                        $"{prefix}Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?"
                },
                new Option
                {
                    Title = $"{prefix}Cetățean cu viză de flotant în orașul în care doresc să votez",
                    Description =
                        $"{prefix}But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?"
                },
                new Option
                {
                    Title = $"{prefix}option 4",
                    Description = $"{prefix}description"
                },
                new Option
                {
                    Title = $"{prefix}option 5",
                    Description = $"{prefix}description"
                },
                new Option
                {
                    Title = $"{prefix}option 6",
                    Description = $"{prefix}description"
                }
            };

            var guide = new VotingGuide
            {
                Description =
                    $"{prefix}Conform legislației electorale, dacă vrei să votezi la alegerile locale ai proceduri diferite prin care îți poți exprima votul. Descoperă mai jos cele șase opțiuni și pașii pe care trebuie să îi urmezi în fiecare dintre cazuri:",
                Options = options
            };

            return guide;
        }

        public StaticData[] LoadStaticData()
        {
            return new[]
            {
                new StaticData
                {
                    Language = Language.Ro,
                    GeneralInfo =
                        "Pe xx mai au loc alegeri locale în România. Românii cu domiciliul sau reședința pe teritoriul Romaniei au dreptul de a-și alege primarii și consilierii locali. Procedurile sunt diferite în funcție de reședință și statutul fiecărui cetățean. Descoperă cu ajutorul Vot România care sunt procedurile pentru a te înregistra să votezi sau de ce acte ai nevoie pentru a vota.",
                    VotersGuide = GetGuide(Language.Ro)
                },
                new StaticData
                {
                    Language = Language.Hu,
                    GeneralInfo =
                        "[HU] Pe xx mai au loc alegeri locale în România. Românii cu domiciliul sau reședința pe teritoriul Romaniei au dreptul de a-și alege primarii și consilierii locali. Procedurile sunt diferite în funcție de reședință și statutul fiecărui cetățean. Descoperă cu ajutorul Vot România care sunt procedurile pentru a te înregistra să votezi sau de ce acte ai nevoie pentru a vota.",
                    VotersGuide = GetGuide(Language.Hu)
                }
            };
        }
    }
}