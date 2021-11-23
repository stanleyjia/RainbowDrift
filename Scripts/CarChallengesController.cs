using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarChallengesController : MonoBehaviour {
    bool chal0;
    bool chal1;
    bool chal3;
    bool chal4;
    bool chal10;
    bool chal11;
    bool chal14;
    bool chal15;
    bool chal17;
    bool chal18;
    bool chal19;
    bool chal23;
    bool chal24;
    bool chal25;
    bool chal26;
    bool chal29;
    bool chal30;
    bool chal31;
    int score;
    Vector3 lastPosition;
    // Use this for initialization
    void Start () {
        chal0 = ChallengesController.ChallengeDone (0);
        chal1 = ChallengesController.ChallengeDone (1);
        chal10 = ChallengesController.ChallengeDone (10);
        chal11 = ChallengesController.ChallengeDone (11);
        chal14 = ChallengesController.ChallengeDone (14);
        chal15 = ChallengesController.ChallengeDone (15);
        chal17 = ChallengesController.ChallengeDone (17);
        chal18 = ChallengesController.ChallengeDone (18);
        chal19 = ChallengesController.ChallengeDone (19);
        chal23 = ChallengesController.ChallengeDone (23);
        chal24 = ChallengesController.ChallengeDone (24);
        chal25 = ChallengesController.ChallengeDone (25);
        chal26 = ChallengesController.ChallengeDone (26);
        chal29 = ChallengesController.ChallengeDone (29);
        chal30 = ChallengesController.ChallengeDone (30);
        chal31 = ChallengesController.ChallengeDone (31);
        lastPosition = transform.position;
        chal4 = ChallengesController.ChallengeDone (4);
        if (chal4) {
            if (DataEntry.instance.userStatistics["GamesPlayed"] >= 9) {
                ChallengesController.CompleteChallenge (4);
                chal4 = false;
            }
        }
        if (chal18) {
            if (DataEntry.instance.userStatistics["GamesPlayed"] >= 300) {
                ChallengesController.CompleteChallenge (18);
                chal18 = false;
            }
        }
        if (chal24) {
            if (DataEntry.instance.userStatistics["GamesPlayed"] >= 1000) {
                ChallengesController.CompleteChallenge (24);
                chal24 = false;
            }
        }
        if (chal30) {
            if (DataEntry.instance.userStatistics["GamesPlayed"] >= 5000) {
                ChallengesController.CompleteChallenge (30);
                chal30 = false;
            }
        }
    }
    // Update is called once per frame
    void Update () {
        //Challenges
        if (CarVariables.instance.gameOn) {
            CheckChallenges ();
            UpdateScore ();
        }
    }
    void UpdateScore () {
        score = CarScoreController.instance.score;
    }
    void CheckChallenges () {
        //Distance
        if (Time.frameCount % 10 == 0) {
            if (chal0) {
                if (score > 300f) {
                    ChallengesController.CompleteChallenge (0);
                    chal0 = false;
                }
            }
            if (chal10) {
                if (score > 600f) {
                    ChallengesController.CompleteChallenge (10);
                    chal10 = false;
                }
            }
            if (chal17) {
                if (score > 1000f) {
                    ChallengesController.CompleteChallenge (17);
                    chal17 = false;
                }
            }
            if (chal23) {
                if (score > 2000f) {
                    ChallengesController.CompleteChallenge (23);
                    chal23 = false;
                }
            }
            if (chal29) {
                if (score > 5000f) {
                    ChallengesController.CompleteChallenge (29);
                    chal29 = false;
                }
            }
        }
        if (Time.frameCount % 10 == 1) {
            if (chal15) {
                if ((CarVariables.instance.distance > 500) && (CoinGeneratorController.instance.noneLost == true)) {
                    ChallengesController.CompleteChallenge (15);
                    chal15 = false;
                }
            }
            if (chal26) {
                if ((CarVariables.instance.distance > 750) && (CoinGeneratorController.instance.noneLost == true)) {
                    ChallengesController.CompleteChallenge (26);
                    chal26 = false;
                }
            }
            if (chal31) {
                if ((CarVariables.instance.distance > 1000) && (CoinGeneratorController.instance.noneLost == true)) {
                    ChallengesController.CompleteChallenge (31);
                    chal31 = false;
                }
            }
        }
        if (Time.frameCount % 10 == 2) {
            if (chal1) {
                if (CoinGeneratorController.instance.coinCount > 30) {
                    ChallengesController.CompleteChallenge (1);
                    chal1 = false;
                }
            }
            if (chal11) {
                if (CoinGeneratorController.instance.coinCount > 100) {
                    ChallengesController.CompleteChallenge (11);
                    chal11 = false;
                }
            }
            if (chal19) {
                if (CoinGeneratorController.instance.coinCount > 200) {
                    ChallengesController.CompleteChallenge (19);
                    chal19 = false;
                }
            }
            if (chal25) {
                if (CoinGeneratorController.instance.coinCount > 500) {
                    ChallengesController.CompleteChallenge (25);
                    chal25 = false;
                }
            }
        }
        if (Time.frameCount % 10 == 3) {
            if (chal14) {
                if (OrbGeneratorController.instance.orbCount > 0) {
                    ChallengesController.CompleteChallenge (14);
                    chal14 = false;
                }
            }
        }
    }
}