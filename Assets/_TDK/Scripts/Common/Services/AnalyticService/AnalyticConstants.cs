namespace _TDK.Common
{
    public class AnalyticConstants
    {
        #region Custom Events

        public const string e_ActionButtonPressed = "ActionButtonPressed";
        public const string e_ComposerBuilt = "ComposerBuilt";
        public const string e_PlayerLogin = "PlayerLogin";
        public const string e_IntroSkipped = "IntroSkipped";
        public const string e_UserReturnedToHomeSceneByPauseMenu = "UserReturnedToHomeSceneByPauseMenu";
        public const string e_ContentStarted = "ContentStarted";
        public const string e_ContentEnded = "ContentEnded";
        public const string e_MoodTrackingSet = "MoodTrackingSet";
        public const string e_CurriculumAnswersSend = "CurriculumAnswersSend";
        public const string e_LicensePurchased = "LicensePurchased";
        public const string e_LicenseConsumed = "LicenseConsumed";

        #endregion

        #region Analytic Parameters

        public const string p_PlayfabID = "playfabID";
        public const string p_UserEmail = "userEmail";
        public const string p_ButtonName = "buttonName";
        public const string p_SceneName = "sceneName";
        public const string p_ContentName = "contentName";
        public const string p_TimeSpentInContent = "timeSpentInContent";
        public const string p_ButtonNameByPlayfabID = "buttonNameByPlayfabID";
        public const string p_ButtonNameByUserEmail = "buttonNameByUserEmail";
        public const string p_UserIDByUserEmail = "userIDByUserEmail";
        public const string p_UserIDByPlayfabID = "userIDByPlayfabID";
        public const string p_TimeSpentAfterUIAppears = "timeSpentAfterUIAppears";
        public const string p_TimeSpentInContentByContentName = "timeSpentInContentByContentName";
        public const string p_IsContentEndedByPauseMenu = "isContentEndedByPauseMenu";

        public const string p_ComposerBenefit = "composerBenefit";
        public const string p_ComposerEnvironment = "composerEnvironment";
        public const string p_ComposerMusic = "composerMusic";

        public const string p_MoodState = "moodState";
        public const string p_MoodContext = "moodContext";
        public const string p_MoodXPosition = "moodXPosition";
        public const string p_MoodyPosition = "moodYPosition";
        public const string p_RelativePosition = "relativePosition";
        public const string p_MoodStateByMoodContext = "moodStateByMoodContext";
        public const string p_MoodStateAndMoodContextByContentName = "moodStateAndMoodContextByContentName";
        public const string p_MoodStateAndMoodContextByUserEmail = "moodStateAndMoodContextByUserEmail";
        public const string p_MoodStateAndMoodContextByPlayfabID = "moodStateAndMoodContextByPlayfabID";
        public const string p_MoodStateByRelativePosition = "moodStateByRelativePosition";
        public const string p_MoodStateAndRelativePositionByContentName = "moodStateAndRelativePositionByContentName";
        public const string p_AllMoodParameters = "allMoodParameters";

        public const string p_CurriculumLifestyleAnswer = "curriculumLifestyleAnswer";
        public const string p_CurriculumExperienceLevelAnswer = "curriculumExperienceLevelAnswer";
        public const string p_CurriculumAvailabityAnswer = "curriculumAvailabilityAnswer";
        public const string p_CurriculumAnswersByUserEmail = "curriculumAnswersByUserEmail";
        public const string p_CurriculumAnswersByPlayfabID = "curriculumAnswersByPlayfabID";

        public const string p_LicenseSku = "licenseSku";
        public const string p_LicensePrice = "licensePrice";
        public const string p_LicensePurchaseDate = "licensePurchaseDate";
        public const string p_LicensePurchaseByPlayfabID = "licensePurchaseByPlayfabID";
        public const string p_LicensePurchaseByEmail = "licensePurchaseByEmail";

        public const string p_LicenseConsumeDate = "licenseConsumeDate";
        public const string p_LicenseConsumeByPlayfabID = "licenseConsumeByPlayfabID";
        public const string p_LicenseConsumeByEmail = "licenseConsumeByEmail";

        #endregion
    }
}