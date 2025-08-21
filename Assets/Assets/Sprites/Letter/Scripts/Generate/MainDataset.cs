using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;


namespace mailGenerator
{
    public class MainDataset : MonoBehaviour
    {
        public RepublikaData RepublikaData;
        public GreschnovaData GreschnovaData;
        public KastavyeData KastavyeData;
        public LetterContentsData LetterContentsData;
        public float universalSize = 3f;

        private void Awake()
        {
            RepublikaData.RepublikaSortData();
            GreschnovaData.GreschnovaSortData();
            KastavyeData.KastavyeSortData();
        }
    }

    ///---------------------------------------

    [Serializable]
    public class RepublikaData
    {
        public GameObject[] RepublikaMail;
        public GameObject[] RepublikaStamps;
        public GameObject[] RepublikaLetters;

        
        public string[] provinceList = { "Priena", "Volknau", "Daravoy", "Reziva", "Discard"};

        //*-------------------

        [HideInInspector] public Dictionary<string, GameObject[]> StampProvinceDictionary;
        public List<provinceNamesStampsClass_Republika> collectStampsFromProvincesClass; //Use to drag-drop stamps and input names of province
        [Serializable]
        public class provinceNamesStampsClass_Republika
        {
            public string provinceName;
            public GameObject[] provinceStamp;
        }

       
        public void IntializeProvinceStampDictionary_Republika()
        {
            if (collectStampsFromProvincesClass == null) return;
            StampProvinceDictionary = new();
            foreach (provinceNamesStampsClass_Republika stamp in collectStampsFromProvincesClass)
            {
                StampProvinceDictionary[stamp.provinceName] = stamp.provinceStamp;
            }
        }

        //*-------------------

        [HideInInspector] [NonSerialized]
        public Dictionary<string, List<string>> LetterContentsDialogueDictionary_Day1 = new()
        {
            ["Priena"] = new List<string>
            {
                "   Hey. I hope the courts aren't drowning you in paperwork like last time.\r\n" +
                "   Do send me a copy of that new legal code when it's stamped, will you? \r\n" +
                "I'm on a bit of a tight schedule, so the sooner the better.\r\n\r\n" +
                "   P.S. Work on that handwriting. Couldn't even tell your address.\r\n" +
                "   P.P.S. Mother says hello.",
                //
                "   Your letter took ages. Did your courier stop to admire St. Helga along the way?\r\n" +
                "   Look, I'm writing to you to say you didn't get the job." +
                "I'm sorry about your dream of being a lawyer, but your application is simply not it.\r\n" +
                "   Write back and let me know you've received this. Quickly.",
                //
                "   This is the twentieth letter this week!\r\n" +
                "   Stop. Complaining. About. The jury. Duty. Summons...At least topics are interesting there. " +
                "I, on the other hand, had to listen to two farmers argue over a goat for three hours.\r\n" +
                "   Every. Week.\r\n" +
                "   And still you think you have it better there in old Priena?\r\n" +
                "   ...\r\n" +
                "   Send wine.\r\n" +
                "   Please.",
                //
                "   Hey, it's been a long time. And look, I need a favor.\r\n" +
                "   You still live near the Petrified Gardens, right? " +
                "Alright, so, I recently got a patron, and she agrees to let my statue display there. " +
                "However, there's been a marble quarry strike, and it's delaying the whole thing. " +
                "And look, I know there's a war going on. But I really, really need this.\r\n" +
                "   Can you… persuade someone? You've got that lawyer glare.\r\n" +
                "   Please? In the name of our friendship?",

                //
                "   Hey kiddo. " +
                "They're requisitioning wagons again. 'For the harvest,' they claim. " +
                "But those roads don't lead to any mill I know.\r\n" +
                "   ...\r\n" +
                "   I hear you're still serving in the Senate. " +
                "A bit of a boring task to folks like me, but what can I say? " +
                "Hey, if you ask your friends in the courts where our wagons really went, that'd be useful.\r\n" +
                "   Make good use of your Pa's money.",
                //
                "   I just got back from my trip at the Old Eyre. " +
                "Strange thing is-the herons aren't nesting near the marshes anymore. " +
                "Too much noise from the west, maybe...\r\n" +
                "   Are you still working at the Ministry of Foreign Affairs? " +
                "Priena's always been good at smoothing things over, right? Any truth to the talk of 'special envoys'?\r\n" +
                "   Write back when you can. Mother and Father are worried sick.",
                //
                "   Not often I write, but I heard a Greschnovan merchant got into a shouting match near the docks yesterday. " +
                "Something about 'neutrality not feeding his children.' " +
                "Makes you wonder how long we can pretend the fighting won't reach us.\r\n" +
                "   Anyway, send more of those thrifted law books of yours. Distractions are welcome these days.",
                //
                "   Hey, I've got a complaint.\r\n" +
                "   The baker's boy got pressed...into Kastavye's army, of all things! " +
                "His family's furious. How's that for 'neutral ground'? " +
                "You're close to the Magistrates, aren't you? Of course, always living in the capital. Finest place away from all troubles.\r\n" +
                "   I'll tell you what. This is unacceptable. And make sure they all know. Now!"
            },

            ["Reziva"] = new List<string>
            {
                "   Hey old friend, " +
                "I bought bananas from your rival port, and you know what? " +
                "Regretted it instantly. Tasted like regret and sour promises. \r\n" +
                "Next time, I'll wait for your plantation's shipment.",
                //
                "   Unacceptable. " +
                "Your market's orange carts are a scam. Three seeds in one fruit? Unacceptable. " +
                "Next time, I'm inspecting each one before I pay.\r\n" +
                "   Also, tell your cousin his 'discount' prices are still robbery.\r\n" +
                "   Robbery!",
                //
                "   I just received your Reziva cider, and I noticed an interesting trend. " +
                "Why does Reziva cider taste always like vinegar but cost like gold?" +
                "I'd rather drink Parakivka's worst vintage. At least theirs doesn't peel paint.\r\n" +
                "   If that's not obvious enough, don't send any more stuff from your backwater province.\r\n" +
                "   Save your money. Goodbye.",
                //
                "   Cousin, I've got a thing to tell you. " +
                "Your dad's 'secret' banana plantation isn't secret. Half the nation's crows are feasting there." +
                "You might want to invest in nets… or a louder scarecrow.",
                //
                "   Insufferable. Absolutely insufferable. " +
                "If I hear one more boast about Reziva's 'legendary' pear cider, I'll scream. " +
                "It's fine. Not worth the ballads.\r\n" +
                "   Send a barrel anyway. At a discount, please?",
                //
                "   The roads near the border are crawling with inspectors. I had to bribe a guard just to send this. " +
                "Well, I hope your orchards are still safe. Good orchards, I tell you.\r\n" +
                "   If the price of peaches spikes again, I swear I'll start smuggling them myself.",
                //
                "   Hey cousin, " +
                "remember that buyer from Vayenne? It turns out he was a quartermaster. " +
                "Now he's demanding 'tribute' in apricots. I told him you switched to growing thorns.\r\n" +
                "   Thank me when you get back. Now spread the lie.",
                //
                "Hello old friend. I hear your lemon farm's struggling. I understand." +
                "The docks are quieter these days. Fewer sailors, more soldiers.\r\n" +
                "   Still, if you've got lemons to spare, I know an officer who'll trade sugar." +
                "Don't ask why he has it, or why he wants lemons. He's an odd gentleman."
            },

            ["Volknau"] = new List<string>
            {
                "   Good day! How have you been?\r\n" +
                "   I wanted to say I saw a flock of starlings heading your way. " +
                "If you bag any, save the wings for my collection.\r\n" +
                "   Also, your 'rare' spotted owl feather was just a dyed pigeon plume." +
                "   Cheeky.",
                //
                "   I write to tell you a miracle!\r\n" +
                "   I found a kingfisher feather by the lake yesterday—blue as your sister's ribbons. " +
                "Well, that's what I wanted to ask. How's Vasilia been?" +
                "Is she still living by Lake Mirren? " +
                "Is she...still available? I think she'll love this feather.\r\n" +
                "   ...\r\n" +
                "   Ah. That was very unproper of me. Anyway, how is life? " +
                "If you're still hunting egrets, save me some for my hat.",
                //
                "   Your cousin's falconry club must be thriving with all these messenger birds getting shot down.\r\n" +
                "   Heard Kastavye's using trained eagles now. Absolute madness.",
                //
                "   Your cousin's new hat is ridiculous. " +
                "Three pheasant tails? Disgusting.\r\n" +
                "   Stop killing birds for fashion. It's abhorrent.",
                //
                "   Don't send me owl feet again. It's gross.",
                //
                "   Hey friend, " +
                "remember that falconer you introduced me to? Turns out his 'trained' hawk just steals passports. " +
                "Mine's gone missing.\r\n" +
                "   If you still live there, send condolences (or a discount on replacements)."
            },

            ["Daravoy"] = new List<string>
            {
                "   I heard the early frost missed your fields this year." +
                "Lucky. Cousin Sergei's weren't so spared.\r\n" +
                "   If your harvest stays strong, save me a bushel. I'll trade you for cider or a favor, whichever's sweeter.",
                //
                "   Remember when we swore we'd never end up like our parents, bent over fields all day? " +
                "Look at us now. At least your soil's fertile. Mine's just stubborn and full of rocks. " +
                "Which, I guess, is why I'm where I am now. Tough luck.\r\n" +
                "   Also, a quick question-do you still sell horses? Write back and let me know.",
                //
                "   I've been hearing rumours." +
                "They're saying Daravoy's grain could feed an army. Let's hope it doesn't come to that.\r\n" +
                "   Either way, save me a sack—I've got a feeling prices will triple by winter.",
                //
                "   Your letters always smell like fresh-cut hay. " +
                "Either you're rolling in the fields when you write, or your whole province is just one big barn.\r\n" +
                "   Miss you so much. Send flour, I'll send bread.",
                //
                "   You're still living in Daravoy right? Listen,..." +
                "Whatever you do, don't argue with the collectors. " +
                "They're calling it a 'temporary levy.' Sounds better than 'theft,' I suppose. " +
                "Just give them the rotten grain.\r\n" +
                "   Trust me.",
                //
                "   Hey, a quick word. " +
                "Remember old man Havel? His farm near the border got 'inspected' by Kastavye troops. " +
                "You're still living on your parents' farm, right? Lock your doors.\r\n" +
                "   And stay sharp, cousin."
            },

            ["Discard"] = new List<string>
            {
                "   Oh good Heavens...\r\n" +
                "   Another week of rain. The roads are mud, the market stalls are flooded, and my socks haven't been dry since Sunday.\r\n" +
                "   When will the sun return? I'm getting so sick of this 'patriotic sacrifice.' " +
                "I swear, when the war's over, I'll be snuggled up in bed for a week.",
                //
                "   Tell your son to stop throwing rocks at my chickens. If he kills one more, I'll report him for 'anti-agrarian sentiment.'\r\n" +
                "   This is your final warning.",
                //
                "   Ode to a Cabbage:\r\n" +
                "   Oh round, oh green, oh slightly bruised,\r\n" +
                "   They say you're 'ration-approved.'\r\n" +
                "   But when I boil you, oh so long,\r\n" +
                "   You still taste oh so wrong.",
                //
                "   You are cordially invited to the annual Office of Postal Standards gala. Theme: 'The Joy of Paperwork.' Bring your own pen.",
                //
                "   Three months waiting for a replacement coat button. THREE MONTHS. " +
                "The 'National Button Office' claims it's 'prioritized for essential personnel.'\r\n" +
                "Since when are buttons a privilege?!\r\n" +
                "   Send me your buttons. NOW!",
                //
                "   I know what you did. Or maybe I don't. But if I did know, you'd be in trouble.\r\n" +
                "   So.\r\n" +
                "   Think about it.",
                //
                "   Your office chair (Serial #4782-XX) has been flagged for 'excessive squeaking.' " +
                "Report to Furniture Compliance for mandatory greasing.\r\n" +
                "   Failure to comply is a fineable offense.",
                //
                "   Saw a man yawn today. Then I yawned. Then the guard yawned. " +
                "Then we all got fined for 'unproductive behavior.'\r\n" +
                "   Curse contagious exhaustion.",
                //
                "   Reminder: Attendance at the 'Voluntary' Folk Dance Rally is mandatory. " +
                "Bring your own ribbons. Lack of enthusiasm will be noted.",
                //
                "   The pigeons outside my window are definitely government spies. Too organized. Too punctual." +
                "Yesterday, one dropped a breadcrumb in perfect Morse code.\r\n" +
                "   Meet me tomorrow. This is worthy of a research paper."
            }
        };
        


        //*-------------------

        public float[] RepublikaMail_Parameters_Continuous;
        [HideInInspector] public GameObject mailBase;

        public float[,] RepublikaMail_Parameters;
        //private string nationName = "Republika";

        public void RepublikaSortData()
        {
            RepublikaMail_Parameters = new float[RepublikaMail.Length, 4];
            recycledMethods.addToMailParameters(RepublikaMail_Parameters, RepublikaMail_Parameters_Continuous, RepublikaMail.Length);
            IntializeProvinceStampDictionary_Republika();
        }

        public int RepublikaMailIndexRandom() => recycledMethods.randomizeMailIndex(RepublikaMail);

        public string RepublikaRandomProvince() => recycledMethods.randomizeString(provinceList);

        public GameObject RepublikaStampRandom(string keyProvince) => recycledMethods.randomizeStamp(StampProvinceDictionary, keyProvince);

        public GameObject RepublikaLetterRandom() => recycledMethods.randomizeGameObject(RepublikaLetters);


    }

    ///---------------------------------------

    [Serializable]
    public class GreschnovaData
    {
        public GameObject[] GreschnovaMail;
        public GameObject[] GreschnovaStamps;
        public GameObject[] GreschnovaLetters;

        public GameObject[] GreschnovaLetters_torn;

        public float[] GreschnovaMail_Parameters_Continuous;
        [HideInInspector] public GameObject mailBase;
        [HideInInspector] public Dictionary<string, GameObject[]> StampProvinceDictionary;
        [HideInInspector] public string[] provinceList = { "St. Siesta", "Borcova", "Bokosnoy", "Parakivka" };

        [Serializable]
        public class provinceNamesStampsClass_Greschnova
        {
            public string provinceName;
            public GameObject[] provinceStamp;
        }

        [Multiline]
        [NonSerialized]
        public string Day1_SpecialGreschnovaLetter =
            "   If this reaches you, burn it after reading.\r\n" +
            "   They’re shelling the docks now. Not just the barracks.\r\n" +
            "   I've moved Marta and the girls to the cellar of the salt warehouse (you know the one), but the patrols grow bolder.\r\n" +
            "   I beg you: use your influence. A transit pass, a bribe, anything to get them across the river to Reziva. " +
            "Don't lie to me. I know you have immense connections there in Priena.\r\n" +
            "   We can pay. Not in coin (they’ve taken that), but the St. Siesta’s blueprints you once asked about. " +
            "Get us out, and they’re yours.\r\n" +
            "   Meet me at the 'Starling' by Old Kraken's Reef. Use the abandoned road to get there.\r\n" +
            "   Come alone. If we’re not there by dawn, assume the worst.";



        public List<provinceNamesStampsClass_Greschnova> collectStampsFromProvincesClass; //Use to drag-drop stamps and input names of province

        public void IntializeProvinceStampDictionary_Greschnova()
        {
            if (collectStampsFromProvincesClass == null) return;
            StampProvinceDictionary = new();
            foreach (provinceNamesStampsClass_Greschnova stamp in collectStampsFromProvincesClass)
            {
                StampProvinceDictionary[stamp.provinceName] = stamp.provinceStamp;
            }
        }

        public float[,] GreschnovaMail_Parameters;
        public void GreschnovaSortData()
        {
            GreschnovaMail_Parameters = new float[GreschnovaMail.Length, 4];
            recycledMethods.addToMailParameters(GreschnovaMail_Parameters, GreschnovaMail_Parameters_Continuous, GreschnovaMail.Length);
            IntializeProvinceStampDictionary_Greschnova();
        }

        public string GreschnovaRandomProvince() => recycledMethods.randomizeString(provinceList);


        public int GreschnovaMailIndexRandom() => recycledMethods.randomizeMailIndex(GreschnovaMail);

        public GameObject GreschnovaStampRandom(string keyProvince) => recycledMethods.randomizeStamp(StampProvinceDictionary, keyProvince);

        public GameObject GreschnovaLetterRandom() => recycledMethods.randomizeGameObject(GreschnovaLetters);

    }

    ///---------------------------------------

    [Serializable]
    public class KastavyeData
    {
        public GameObject[] KastavyeMail;
        public GameObject[] KastavyeStamps;
        public GameObject[] KastavyeLetters;

        [HideInInspector] public Dictionary<string, GameObject[]> StampProvinceDictionary;
        [HideInInspector] public string[] provinceList = { "Bel Antienne", "Vayenne" };

        [Serializable]
        public class provinceNamesStampsClass_Kastavye
        {
            public string provinceName;
            public GameObject[] provinceStamp;
        }


        public List<provinceNamesStampsClass_Kastavye> collectStampsFromProvincesClass; //Use to drag-drop stamps and input names of province

        public void IntializeProvinceStampDictionary_Kastavye()
        {
            if (collectStampsFromProvincesClass == null) return;
            StampProvinceDictionary = new();
            foreach (provinceNamesStampsClass_Kastavye stamp in collectStampsFromProvincesClass)
            {
                StampProvinceDictionary[stamp.provinceName] = stamp.provinceStamp;
            }
        }

        


        public float[] KastavyeMail_Parameters_Continuous;
        [HideInInspector] public GameObject mailBase;

        public float[,] KastavyeMail_Parameters;
        //private string nationName = "Kastavye";
        public void KastavyeSortData()
        {
            KastavyeMail_Parameters = new float[KastavyeMail.Length, 4];
            recycledMethods.addToMailParameters(KastavyeMail_Parameters, KastavyeMail_Parameters_Continuous, KastavyeMail.Length);
            IntializeProvinceStampDictionary_Kastavye();
        }

        public int KastavyeMailIndexRandom() => recycledMethods.randomizeMailIndex(KastavyeMail);

        public string KastavyeRandomProvince() => recycledMethods.randomizeString(provinceList);

        public GameObject KastavyeStampRandom(string keyProvince) => recycledMethods.randomizeStamp(StampProvinceDictionary, keyProvince);

        public GameObject KastavyeLetterRandom() => recycledMethods.randomizeGameObject(KastavyeLetters);

    }

    ///---------------------------------------


    internal class recycledMethods {
        public static void addToMailParameters(float[,] parameterList, float[] parameter_continuousList, int mailListLength)
        {
            int i = 0;
            int j = 0;
            foreach (float parameter in parameter_continuousList)
            {
                if (i >= mailListLength) return;
                parameterList[i, j] = parameter;
                j++;
                if (j >= 4)
                {
                    j = 0;
                    i++;
                }

            }
        }

        [HideInInspector] public static int randomizeMailIndex(GameObject[] mailList)
        {
            if (mailList.Length == 1) return 0;
            return UnityEngine.Random.Range(0, mailList.Length);
        }

        [HideInInspector] public static GameObject randomizeStamp(Dictionary<string, GameObject[]> provinceStampDictionary, string keyProvince)
        {
            return provinceStampDictionary[keyProvince][UnityEngine.Random.Range(0,2)];
        }

        [HideInInspector] public static string randomizeString(string[] stringArray)
        {
            if (stringArray.Length == 1) return stringArray[0];
            return stringArray[UnityEngine.Random.Range(0, stringArray.Length)];
        }

        [HideInInspector] public static GameObject randomizeGameObject(GameObject[] gameObjectList) {
            if (gameObjectList == null) return null;
            return gameObjectList[UnityEngine.Random.Range(0, gameObjectList.Length)];
        }

        

    }


    ///---------------------------------------
    [Serializable]
    public class LetterContentsData
    {

        public List<List<string>> Republika_ToPriena_Normal = new();



        [HideInInspector] public string[] firstNames = new string[] {
            "Miron", "Darya", "Yeva", "Kir", "Gora", "Valeriy", "Lev", "Anastasiya", "Aksinya", "Apostol", "Natasha",
            "Sergei", "Zhanna", "Zinoviya", "Grigoriy", "Alyosha", "Rolan", "Belova", "Roza", "Iolanta", "Andrei",
            "Ipatiy", "Vasily", "Vasya", "Yuri", "Yustina", "Irinushka", "Pavel", "Petre", "Viktor", "Raisa",
            "Avdotia", "Dima", "Patya", "Stephan", "Demya", "Augustin", "Maksim", "Zinoviy", "Boris", "Matvei",
            "Savya", "Sasha", "Petya", "Vaughn", "Volkovi", "Emorlai", "Yaroslav", "Grisha", "Matryosha", "Raisa",
            "Gavriila", "Klara", "Larissa", "Alyona", "Klavdiya", "Yelena", "Makarova", "Nonna", "Klava", "Irina",
            "Magdalina", "Aksinya", "Talya", "Favora", "Rosi", "Lyuba", "Pelageya", "Vlad", "Vladima", "Luka", "Stepan",
            "Shura", "Vasilieva", "Magdalene", "Achima", "Ebbe", "Sophia", "Karla", "Liesl", "Erma", "Fritz", "Fitzgerald",
            "Adalinda", "Heiden", "Rosamond", "Sigi", "Lorelei", "Pierre", "Albain", "Tristan", "Leona", "Antonin", "Severin",
            "Alphonse", "Ace", "Rodrigue", "Jeremie", "Arrok", "Quinton", "Sterling", "Leandre", "Beaumont", "Jeremie", "Otan",
            "Émile", "Bartholomieu", "Urbain", "Pierres", "Yves", "Fay", "Claude", "Etienne", "Cesaire", "Saphau", "Alain", "Geraud",
            "Corin", "Nihel", "Felix", "Sylvain", "Paschal", "Julian", "Julienne", "Adolphe", "Salomon", "Raimond", "Alard", "Napoli",
            "Aurele", "Stuart", "Percevel", "Sylvestre", "Acelet", "Liliane", "Magali", "Margaux", "Bonnie", "Bonnet", "Rosine",
            "Cecile", "Dione", "Berdine", "Andree", "Idelle", "Regine", "Heloise", "Sidonie", "Evonnie", "Cerise", "Christelle",
            "Cristina", "Matilde", "Mireio", "Iseult", "Blanche", "Helewise", "Emmy", "Sacha", "Edith", "Finch", "Eda", "Ena",
            "Dianne", "Titania", "Amorette", "Sebastienne", "Ygraine", "Amarante", "Gaelle", "Lillian", "Cyrille", "Celeste",
            "Tasse", "Morgause", "Regine", "Dennel", "Charron", "Elisabetha", "Faustine", "Dmitri"
        };

        [HideInInspector] public string[] lastNames = new string[]
        {
            "Chevalier", "Gosselin", "Fontaine", "Martin", "Rayer", "Rabien", "Roussel", "Jacques", "Linville", "Marine",
            "Porcher", "Young", "Service", "Faure", "Fourier", "Duval", "Travere", "Mercier", "Belmont", "Tide", "Rousseau",
            "Chaput", "Tran", "Olivier", "Janvier", "Mathieu", "Blanc", "Zaytsev", "Vasiliev", "Stepanova", "Borisov", "Gusteau",
            "Aleks", "Sokolov", "Sasha", "Semenova", "Kuznet", "Kulikov", "Zatyse", "Morozova", "Ilina", "Belova", "Korolev",
            "Tarasov", "Baranov", "Kiselev", "Alexeev", "Makarov", "Grigorev", "Semyon", "Volkov", "Vasilieva", "Popoa", "Baranova",
            "Sergeeva", "Frolova", "Dmitrieva", "Koroleva", "Soloveva", "Peraska", "Peralta", "Antonia", "Antonokiv", "Adamic",
            "Alekseyev", "Benes", "Beras", "Cerna", "Cerny", "Chavdarova", "Dragov", "Dragovini", "Fedorenko", "Fedorov",
            "Filip", "Gasper", "Genov", "Gorski", "Horace", "Smith", "Ilic", "Chanty", "Iliev", "Isaev", "Isakova", "Ivanova",
            "Ivanovic", "Jahua", "Ivov", "Ivano", "Jalen", "Kafka", "Kaspar", "Kavaliova", "Kavaly", "Kiesk", "Klement", "Kravets",
            "Kyssa", "Kysela", "Laska", "Marek", "Marinova", "Markov", "Nguyen", "Markova", "Maroz", "Markovic", "Martinov", "Masek",
            "Mateev", "Medev", "Medeya", "Michalska", "Naumov", "Milic", "Moravec", "Morakova", "Navratil", "Nemec", "Nikolov",
            "Niccolo", "Novikov", "Orlov", "Pavloka", "Pavlovic", "Pavlovsky", "Peric", "Pekorvic", "Peskova", "Petrenko", "Pentric",
            "Petrov", "Popov", "Radev", "Radic", "Rehova", "Reznic", "Romanov", "Rudaski", "Ryab", "Rye", "Saric", "Sergeev", "Sevcik",
            "Sikora", "Simek", "Simeonov", "Skala", "Sitco", "Slako", "Sokolov", "Stanek", "Stefan", "Tsera", "Tomov", "Tomic", "Utkina",
            "Vacek", "Utkin", "Valent", "Valoska", "Vancheva", "Vanchev", "Vaneva", "Vasylyk", "Vesela", "Vysiliv", "Vidmar", "Vidomic",
            "Vlasak", "Vinkovic", "Wilk", "Yanev", "Yvanna", "Yegorov", "Yegora", "Zajic", "Zajac", "Zayzetsa", "Zelenko", "Zelenka", "Levi",
            "Amiranda", "Zukov", "Zoric", "Zora", "Zelinski"
        };

        [HideInInspector] public int randomizeIndexList(List<string[]> dialogueList)
        {

            if (dialogueList.Count == 1) return 0;
            return UnityEngine.Random.Range(0, dialogueList.Count);
        }

        [HideInInspector] public string randomNameGenerator(string[] nameArray)
        {
            return nameArray[UnityEngine.Random.Range(0, nameArray.Length)];
        }

    }

}