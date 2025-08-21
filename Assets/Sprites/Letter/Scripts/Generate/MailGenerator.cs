using mailGenerator;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.Rendering;

namespace mailGenerator
{
    //Generates mail and their appropriate stamps and letter sprite
    //(since each nation has different colors, their letters and stamps are unique as well)
    //Draws data from MainDataset script to procedurally generate a new piece of mail and its content each time
    //Contains methods with weighted random selection
    public class MailGenerator : MonoBehaviour
    {

        // Position parameters for stamp placement
        private float xLeft;
        private float xRight;
        private float yUp;
        private float yDown;

        // Nation data
        protected int nationRandom; // Randomly selected nation index
        [HideInInspector] public int MailIndex; // Index of the selected mail prefab
        [HideInInspector] public string SenderNationName; // Name of the selected nation
        [HideInInspector] public string SenderProvinceName; //Name of the select (randomized) province; based on nation

        [HideInInspector] public string ReceiverNationName; // Name of the selected nation
        [HideInInspector] public string ReceiverProvinceName; //Name of the select (randomized) province; based on nation

        // GameObjects
        private GameObject _mailPrefab; // Selected mail prefab to instantiate
        private GameObject _stampPrefab; // Selected stamp prefab to instantiate
        private GameObject _parentGroup; // Parent object for organizing instantiated mai
        protected ScoreTracker _scoreTracker;
        private GameObject _mailClone = null;

        // Data references
        public MainDataset MainDataset; // Main dataset containing all nation data
        private RepublikaData _republikaData; // Republika nation data
        private GreschnovaData _greschnovaData; // Greschnova nation data
        private KastavyeData _kastavyeData; // Kastavye nation data
        

        [SerializeField] private int _totalNations = 3; // Total number of nations available
        [SerializeField] private float _stampAngleMax = 10f; //Biggest absolute value of stamp angle
        [SerializeField] private Transform _mailSpawnPos;
        private AudioSourcePool _audioSourcePool;


        // Nation index list:
        // Republika = 0
        // Greschnova = 1
        // Kastavye = 2


        // Initializes data connections on awake
        private void Awake()
        {
            _cacheData();
            _connectDataToBase();
            _sortNationData();
        }

        private void _cacheData()
        {
            _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker")?.GetComponent<ScoreTracker>();
            _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
            _parentGroup = GameObject.FindGameObjectWithTag("MoveableArea");
        }

        // Connects all nation data references to the main dataset
        private void _connectDataToBase()
        {
            _republikaData = MainDataset.RepublikaData;
            _greschnovaData = MainDataset.GreschnovaData;
            _kastavyeData = MainDataset.KastavyeData;
        }

        private void _sortNationData()
        {
            _republikaData.RepublikaSortData();
            _greschnovaData.GreschnovaSortData();
            _kastavyeData.KastavyeSortData();
        }

        // *------------------ MAIL RELATED METHODS --------------------

        public void GenerateMail()
        {
            int mailCounter = _scoreTracker.MailCounter;
            if (_checkSpecialMail()) return;
            GenerateRandomMail();

            //-----

            bool _checkSpecialMail(){

                

                if (_scoreTracker.DayNum == 1)
                {
                    if (mailCounter + 1 == _scoreTracker.MailGoal)
                    {
                        SpecialDay1_GenerateGreschnovaMail();
                        return true;
                    }
                    if (mailCounter + 1 == 2 || mailCounter + 1 == 5 || mailCounter + 1 == 7)
                    {
                        Day1_GenerateRepublikaMail_Tutorial();
                        return true;
                    }
                    if (mailCounter + 1 == 3 || mailCounter + 1 == 6)
                    {
                        Day1_GenerateRepublikaMail_Faulty();
                        return true;
                    }

                    Day1_GenerateRepublikaMail();
                    return true;
                }
                Debug.Log("No Special Mail");
                return false;
            }
        }

        private int _returnRandomNation()
        {
            return Random.Range(0, _totalNations);
        }


        // Generates Republika mail for day 1
        [HideInInspector] public void Day1_GenerateRepublikaMail()
        {
            _SetUp_Nation_Province_MailPrefab(0, true, true, false, false);
            _generateReceiverInfo(true, 0);
            _createMailWithStamp();
        }

        // Generates Republika mail for day 1
        // MAIL ALL CORRECT
        [HideInInspector]
        public void Day1_GenerateRepublikaMail_Tutorial()
        {
            _SetUp_Nation_Province_MailPrefab(0, true, true, true, false);
            _generateReceiverInfo(true, 0);
            _createMailWithStamp();
        }

        [HideInInspector]
        public void Day1_GenerateRepublikaMail_Faulty()
        {
            _SetUp_Nation_Province_MailPrefab(0, true, true, false, true);
            _generateReceiverInfo(true, 0);
            _createMailWithStamp();
        }

        //Special generation
        [HideInInspector] public void SpecialDay1_GenerateGreschnovaMail() {
            _SetUp_Nation_Province_MailPrefab(1, true, false, true, false, null, "Borcova");
            _generateReceiverInfo(false, 0, "Republika", "Priena");
            _createMailWithStamp();
            _mailClone.GetComponent<MailTornAppearance>().IsMailTornAppearnace = true;
        }

        // Public entry point to start the mail generation process with random nation selection
        public void GenerateRandomMail()
        {
            _SetUp_Nation_Province_MailPrefab(_returnRandomNation(), true, true, false, false);
            _generateReceiverInfo(true, _returnRandomNation());
            _createMailWithStamp();

        }


        //It sets up sender's nation, province, and mail prefab
        //Option for custom province and mail prefabs
        private void _SetUp_Nation_Province_MailPrefab(int nation, bool _isRandomMailPrefab, bool _isRandomProvinceName, bool _isAllCorrect, bool _isFaulty, GameObject _customMailPrefab = null, string _customSenderProvinceName = null)
        {
            switch (nation)
            {
                case 0:
                    _setupNationRepublika(_isRandomMailPrefab, _isRandomProvinceName, _isAllCorrect, _isFaulty, _customMailPrefab, _customSenderProvinceName);
                    return;
                case 1:
                    _setupNationGreschnova(_isRandomMailPrefab, _isRandomProvinceName, _isAllCorrect, _isFaulty, _customMailPrefab, _customSenderProvinceName);
                    return;
                case 2:
                    _setupNationKastavye(_isRandomMailPrefab, _isRandomProvinceName, _isAllCorrect, _isFaulty, _customMailPrefab, _customSenderProvinceName);
                    return;
            }
        }

        //METHODS FOR SETTING UP NATIONS HERE
        #region "Setup Nations" 
        private void _setupNationRepublika(bool _isRandomMailPrefab, bool _isRandomProvinceName, bool _isAllCorrect, bool _isFaulty, GameObject _customMailPrefab = null, string _customSenderProvinceName = null)
        {
            SenderNationName = "Republika";

            if (!_isRandomMailPrefab) {
                _mailPrefab = _customMailPrefab;
            }
            else
            {
                MailIndex = _republikaData.RepublikaMailIndexRandom();
                _mailPrefab = _republikaData.RepublikaMail[MailIndex];
            }

            if (!_isRandomProvinceName) {
                SenderProvinceName = _customSenderProvinceName;
            }
            else
            {
                if (_isAllCorrect)
                {
                    SenderProvinceName = _republikaData.RepublikaRandomProvince();
                    while (SenderProvinceName == "Discard")
                    {
                        SenderProvinceName = _republikaData.RepublikaRandomProvince();
                    }
                }
                else if (_isFaulty)
                {
                    SenderProvinceName = "Discard";
                }
                else SenderProvinceName = _republikaData.RepublikaRandomProvince();
            }


            _stampPrefab = _republikaData.RepublikaStampRandom(SenderProvinceName);
            _getMailParameters(_republikaData.RepublikaMail_Parameters); //void method, changes values of x and y
        }

        

        private void _setupNationGreschnova(bool _isRandomMailPrefab, bool _isRandomProvinceName, bool _isAllCorrect, bool _isFaulty, GameObject _customMailPrefab = null, string _customSenderProvinceName = null)
        {
            SenderNationName = "Greschnova";

            if (!_isRandomMailPrefab)
            {
                _mailPrefab = _customMailPrefab;
            }
            else
            {

                MailIndex = _greschnovaData.GreschnovaMailIndexRandom();
                _mailPrefab = _greschnovaData.GreschnovaMail[MailIndex];
            }

            if (!_isRandomProvinceName)
            {
                SenderProvinceName = _customSenderProvinceName;
            }
            else
            {

                if (_isAllCorrect)
                {
                    SenderProvinceName = _greschnovaData.GreschnovaRandomProvince();
                    while (SenderProvinceName == "Discard")
                    {
                        SenderProvinceName = _greschnovaData.GreschnovaRandomProvince();
                    }
                }
                else if (_isFaulty)
                {
                    SenderProvinceName = "Discard";
                }
                else SenderProvinceName = _greschnovaData.GreschnovaRandomProvince();
            }
            _stampPrefab = _greschnovaData.GreschnovaStampRandom(SenderProvinceName);
            _getMailParameters(_greschnovaData.GreschnovaMail_Parameters); //void method, changes values of x and y
        }
        private void _setupNationKastavye(bool _isRandomMailPrefab, bool _isRandomProvinceName, bool _isAllCorrect, bool _isFaulty, GameObject _customMailPrefab = null, string _customSenderProvinceName = null)
        {
            SenderNationName = "Kastavye";
            if (!_isRandomMailPrefab)
            {
                _mailPrefab = _customMailPrefab;
            }
            else
            {
                MailIndex = _kastavyeData.KastavyeMailIndexRandom();
                _mailPrefab = _kastavyeData.KastavyeMail[MailIndex];
            }

            if (!_isRandomProvinceName)
            {
                SenderProvinceName = _customSenderProvinceName;
            }
            else
            {

                if (_isAllCorrect)
                {
                    SenderProvinceName = _kastavyeData.KastavyeRandomProvince();
                    while (SenderProvinceName == "Discard")
                    {
                        SenderProvinceName = _kastavyeData.KastavyeRandomProvince();
                    }
                }
                else if (_isFaulty)
                {
                    SenderProvinceName = "Discard";
                }
                else SenderProvinceName = _kastavyeData.KastavyeRandomProvince();
            }
            _stampPrefab = _kastavyeData.KastavyeStampRandom(SenderProvinceName);
            _getMailParameters(_kastavyeData.KastavyeMail_Parameters); //void method, changes values of x and y
        }


        #endregion

        private void _generateReceiverInfo(bool _isRandomAll, int _receiverNation, string _receiverNationName = null, string _receiverProvinceName = null)
        {
            if (!_isRandomAll)
            {
                ReceiverNationName = _receiverNationName;
                ReceiverProvinceName = _receiverProvinceName;
                return;
            }
            switch (_receiverNation) {
                case 0:
                    ReceiverNationName = "Republika";
                    ReceiverProvinceName = _republikaData.RepublikaRandomProvince();
                    return;
                case 1:
                    ReceiverNationName = "Greschnova";
                    ReceiverProvinceName = _greschnovaData.GreschnovaRandomProvince();
                    return;
                case 2:
                    ReceiverNationName = "Kastavye";
                    ReceiverProvinceName = _kastavyeData.KastavyeRandomProvince();
                    return;
            }
        }
        


        // Creates the mail GameObject with a stamp at a random position
        private void _createMailWithStamp()
        {
            Vector3 stampPosition = _getRandomStampPosition();

            
            _mailClone = Instantiate(_mailPrefab, _mailSpawnPos.position, Quaternion.identity);
            _mailClone.name = "Mail";
            _scoreTracker.MailCounter++;
            _audioSourcePool.SFX_PaperSlide.Play();

            MailProperties _mailCloneProperties = _mailClone.GetComponent<MailProperties>();

            _mailCloneProperties.Local_senderNationName = SenderNationName;
            _mailCloneProperties.Local_senderProvinceName = SenderProvinceName;
            _mailCloneProperties.Local_receiverNationName = ReceiverNationName;
            _mailCloneProperties.Local_receiverProvinceName = ReceiverProvinceName;

            _createStampClone(_mailClone, stampPosition);
            _setupMailCloneProperties(_mailClone);
        }


        // Generates a random stamp position within the mail's defined boundaries
        private Vector3 _getRandomStampPosition()
        {
            float xPos = Random.Range(xLeft, xRight);
            float yPos = Random.Range(yUp, yDown);
            return new Vector3(xPos, yPos, 0f);
        }

        // Creates and positions the stamp on the mail
        private void _createStampClone(GameObject mailClone, Vector3 position)
        {
            GameObject stampClone = Instantiate(
                _stampPrefab,
                transform.position,
                Quaternion.identity,
                mailClone.transform
            );
            stampClone.transform.localPosition = position;

            float rotateX = Random.Range(-_stampAngleMax, _stampAngleMax); //Randomize the angle of the stamp (for natural look)
            stampClone.transform.localRotation = Quaternion.Euler(0f, 0f, rotateX);
        }

        // Configures properties of the mail clone (size, parenting, etc.)
        private void _setupMailCloneProperties(GameObject mailClone)
        {
            float cloneSize = MainDataset.universalSize;
            mailClone.transform.localScale = Vector3.one * cloneSize;
           
            mailClone.transform.SetParent(_parentGroup.transform);
            mailClone.transform.SetAsLastSibling();
        }

        // Retrieves position parameters for stamp placement based on mail index
        private void _getMailParameters(float[,] parameters)
        {
            xLeft = parameters[MailIndex, 0];
            xRight = parameters[MailIndex, 1];
            yUp = parameters[MailIndex, 2];
            yDown = parameters[MailIndex, 3];
        }

        [HideInInspector]
        public void ChangeMailSpawnPos()
        {
            _mailSpawnPos.position = new(-3f, _mailSpawnPos.position.y);
        }
    }
}