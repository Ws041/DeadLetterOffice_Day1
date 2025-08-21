using System.Collections;
using System.Collections.Generic;
using mailGenerator;
using UnityEngine;

namespace mailGenerator
{
    public class MailProperties : MailGenerator
    {
        private string _local_receiverProvinceName, _local_senderProvinceName;

        public string Local_senderName, Local_senderNationName;
        public string Local_receiverName, Local_receiverNationName;

        private MainDataset _mainDataset;
        private LetterContentsData _letterContentsData;

        private void Awake()
        {
            _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
            _mainDataset = GameObject.FindGameObjectWithTag("MailGenerator").GetComponent<MailGenerator>().MainDataset;
            _letterContentsData = _mainDataset.LetterContentsData;
        }

        private void Start()
        {
            _checkSpecialNames();
        }
        public string Local_receiverProvinceName {
            get => _local_receiverProvinceName;
            set {
                if (Local_senderProvinceName == "Discard")
                {
                    _local_receiverProvinceName = "Discard";
                    return;
                }
                _local_receiverProvinceName = value;
            }
        }

        public string Local_senderProvinceName {
            get => _local_senderProvinceName;
            set
            {
                _local_senderProvinceName = value;
            }
        }

        private void _checkSpecialNames()
        {
            if (_scoreTracker.DayNum == 1 && _scoreTracker.MailCounter == _scoreTracker.MailGoal)
            {
                _assignNames(false, "Grigoriy", "Dmitri");
                return;
            }
            _assignNames(true);
        }

        private void _assignNames(bool _isRandom, string _customSenderName = null, string _customReceiverName = null)
        {
            if (!_isRandom)
            {
                Local_senderName = _customSenderName;
                Local_receiverName = _customReceiverName;
            }
            else
            {
                _randomizeName(ref Local_senderName);
                _randomizeName(ref Local_receiverName);
            }
        }

        // Randomizes a name with 50% chance for formal/informal variants
        private void _randomizeName(ref string name)
        {
            int _rand = Random.Range(0, 2);
            if (_rand == 0) // Informal (first name only)
            {
                name = _letterContentsData.randomNameGenerator(_letterContentsData.firstNames);
            }
            else // Formal (title + last name)
            {
                int _randGender = Random.Range(0, 3);
                name = _randGender switch
                {
                    0 => "Mr. ",
                    1 => "Mrs. ",
                    2 => "Ms. ",
                    _ => name
                };
                name += _letterContentsData.randomNameGenerator(_letterContentsData.lastNames);
            }
        }

    }
}