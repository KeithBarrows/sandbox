using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectionTracker.NyTimes
{
    // Live Updates - Grand Children...
    public class Winner
    {
        [JsonPropertyName("candidate_id")]
        public string CandidateId { get; set; }

        [JsonPropertyName("candidate_key")]
        public string CandidateKey { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("name_display")]
        public string NameDisplay { get; set; }

        [JsonPropertyName("party_id")]
        public string PartyId { get; set; }

        [JsonPropertyName("incumbent")]
        public bool Incumbent { get; set; }

        [JsonPropertyName("runoff")]
        public bool Runoff { get; set; }

        [JsonPropertyName("winner")]
        public bool WinnerField { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("percent")]
        public double Percent { get; set; }

        [JsonPropertyName("percent_display")]
        public string PercentDisplay { get; set; }

        [JsonPropertyName("electoral_votes")]
        public int ElectoralVotes { get; set; }

        [JsonPropertyName("absentee_votes")]
        public int AbsenteeVotes { get; set; }

        [JsonPropertyName("absentee_percent")]
        public double AbsenteePercent { get; set; }

        [JsonPropertyName("img_url")]
        public string ImgUrl { get; set; }

        [JsonPropertyName("has_image")]
        public bool HasImage { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("pronoun")]
        public string Pronoun { get; set; }
    }
    public class LiveUpdate
    {
        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonPropertyName("include_on_homepage")]
        public bool IncludeOnHomepage { get; set; }

        [JsonPropertyName("include_in_reporter_updates_feed")]
        public bool IncludeInReporterUpdatesFeed { get; set; }

        [JsonPropertyName("office")]
        public string Office { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("call_type")]
        public string CallType { get; set; }

        [JsonPropertyName("race_id")]
        public string RaceId { get; set; }

        [JsonPropertyName("datetime")]
        public object Datetime { get; set; }

        [JsonPropertyName("is_today")]
        public bool IsToday { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("winner")]
        public Winner Winner { get; set; }

        [JsonPropertyName("party_id")]
        public string PartyId { get; set; }

        [JsonPropertyName("candidate_last_name")]
        public string CandidateLastName { get; set; }

        [JsonPropertyName("candidate_name_display")]
        public string CandidateNameDisplay { get; set; }

        [JsonPropertyName("candidate_id")]
        public string CandidateId { get; set; }

        [JsonPropertyName("race_call_party_winner")]
        public string RaceCallPartyWinner { get; set; }

        [JsonPropertyName("state_name")]
        public string StateName { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("author_title_or_location")]
        public string AuthorTitleOrLocation { get; set; }

        [JsonPropertyName("link_url")]
        public string LinkUrl { get; set; }

        [JsonPropertyName("link_text")]
        public string LinkText { get; set; }

        [JsonPropertyName("linked_state_1")]
        public string LinkedState1 { get; set; }

        [JsonPropertyName("linked_state_2")]
        public string LinkedState2 { get; set; }

        [JsonPropertyName("linked_state_3")]
        public string LinkedState3 { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("image_credit")]
        public string ImageCredit { get; set; }

        [JsonPropertyName("author_headshot")]
        public string AuthorHeadshot { get; set; }

        [JsonPropertyName("linked_states")]
        public object LinkedStates { get; set; }
    }

    // Party Control - Grand Children...
    public class NoElection
    {
        [JsonPropertyName("democrat")]
        public int? Democrat { get; set; }

        [JsonPropertyName("republican")]
        public int? Republican { get; set; }

        [JsonPropertyName("other")]
        public int? Other { get; set; }
    }
    public class Democrat
    {
        [JsonPropertyName("party_id")]
        public string PartyId { get; set; }

        [JsonPropertyName("name_display")]
        public string NameDisplay { get; set; }

        [JsonPropertyName("name_abbr")]
        public string NameAbbr { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("percent")]
        public string Percent { get; set; }

        [JsonPropertyName("change")]
        public int Change { get; set; }

        [JsonPropertyName("flip_count")]
        public int FlipCount { get; set; }

        [JsonPropertyName("leader_count")]
        public int LeaderCount { get; set; }

        [JsonPropertyName("change_full")]
        public string ChangeFull { get; set; }

        [JsonPropertyName("change_abbr")]
        public string ChangeAbbr { get; set; }

        [JsonPropertyName("flip_text_full")]
        public string FlipTextFull { get; set; }

        [JsonPropertyName("flip_text_extra")]
        public string FlipTextExtra { get; set; }

        [JsonPropertyName("flip_text_abbr")]
        public string FlipTextAbbr { get; set; }

        [JsonPropertyName("winner")]
        public bool Winner { get; set; }
    }
    public class Republican
    {
        [JsonPropertyName("party_id")]
        public string PartyId { get; set; }

        [JsonPropertyName("name_display")]
        public string NameDisplay { get; set; }

        [JsonPropertyName("name_abbr")]
        public string NameAbbr { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("percent")]
        public string Percent { get; set; }

        [JsonPropertyName("change")]
        public int Change { get; set; }

        [JsonPropertyName("flip_count")]
        public int FlipCount { get; set; }

        [JsonPropertyName("leader_count")]
        public int LeaderCount { get; set; }

        [JsonPropertyName("change_full")]
        public string ChangeFull { get; set; }

        [JsonPropertyName("change_abbr")]
        public string ChangeAbbr { get; set; }

        [JsonPropertyName("flip_text_full")]
        public string FlipTextFull { get; set; }

        [JsonPropertyName("flip_text_extra")]
        public string FlipTextExtra { get; set; }

        [JsonPropertyName("flip_text_abbr")]
        public string FlipTextAbbr { get; set; }

        [JsonPropertyName("winner")]
        public bool Winner { get; set; }
    }
    public class Other
    {
        [JsonPropertyName("party_id")]
        public string PartyId { get; set; }

        [JsonPropertyName("name_display")]
        public string NameDisplay { get; set; }

        [JsonPropertyName("name_abbr")]
        public string NameAbbr { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("percent")]
        public string Percent { get; set; }

        [JsonPropertyName("change")]
        public int Change { get; set; }

        [JsonPropertyName("flip_count")]
        public int FlipCount { get; set; }

        [JsonPropertyName("leader_count")]
        public int LeaderCount { get; set; }

        [JsonPropertyName("change_full")]
        public string ChangeFull { get; set; }

        [JsonPropertyName("change_abbr")]
        public string ChangeAbbr { get; set; }

        [JsonPropertyName("flip_text_full")]
        public string FlipTextFull { get; set; }

        [JsonPropertyName("flip_text_extra")]
        public string FlipTextExtra { get; set; }

        [JsonPropertyName("flip_text_abbr")]
        public string FlipTextAbbr { get; set; }

        [JsonPropertyName("winner")]
        public bool Winner { get; set; }
    }
    public class Parties
    {
        [JsonPropertyName("democrat")]
        public Democrat Democrat { get; set; }

        [JsonPropertyName("republican")]
        public Republican Republican { get; set; }

        [JsonPropertyName("other")]
        public Other Other { get; set; }
    }
    public class PartyControl
    {
        [JsonPropertyName("race_type")]
        public string RaceType { get; set; }

        [JsonPropertyName("state_id")]
        public string StateId { get; set; }

        [JsonPropertyName("needed_for_control")]
        public int NeededForControl { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("no_election")]
        public NoElection NoElection { get; set; }

        [JsonPropertyName("winner")]
        public object Winner { get; set; }

        [JsonPropertyName("parties")]
        public Parties Parties { get; set; }

        [JsonPropertyName("winnerCalledTimestamp")]
        public object WinnerCalledTimestamp { get; set; }
    }


    // Races - Grand Children...
    public class Candidate
    {
        [JsonPropertyName("candidate_id")]
        public string CandidateId { get; set; }

        [JsonPropertyName("candidate_key")]
        public string CandidateKey { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("name_display")]
        public string NameDisplay { get; set; }

        [JsonPropertyName("party_id")]
        public string PartyId { get; set; }

        [JsonPropertyName("incumbent")]
        public bool Incumbent { get; set; }

        [JsonPropertyName("runoff")]
        public bool Runoff { get; set; }

        [JsonPropertyName("winner")]
        public bool Winner { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("percent")]
        public double Percent { get; set; }

        [JsonPropertyName("percent_display")]
        public string PercentDisplay { get; set; }

        [JsonPropertyName("electoral_votes")]
        public int ElectoralVotes { get; set; }

        [JsonPropertyName("absentee_votes")]
        public int AbsenteeVotes { get; set; }

        [JsonPropertyName("absentee_percent")]
        public double AbsenteePercent { get; set; }

        [JsonPropertyName("img_url")]
        public string ImgUrl { get; set; }

        [JsonPropertyName("has_image")]
        public bool HasImage { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("pronoun")]
        public string Pronoun { get; set; }
    }
    public class Results
    {
        [JsonPropertyName("bidenj")]
        public int Bidenj { get; set; }

        [JsonPropertyName("trumpd")]
        public int Trumpd { get; set; }

        [JsonPropertyName("jorgensenj")]
        public int Jorgensenj { get; set; }

        [JsonPropertyName("write-ins")]
        public int? WriteIns { get; set; }
    }
    public class ResultsAbsentee
    {
        [JsonPropertyName("bidenj")]
        public int Bidenj { get; set; }

        [JsonPropertyName("trumpd")]
        public int Trumpd { get; set; }

        [JsonPropertyName("jorgensenj")]
        public int Jorgensenj { get; set; }

        [JsonPropertyName("write-ins")]
        public int? WriteIns { get; set; }
    }
    public class County
    {
        [JsonPropertyName("fips")]
        public string Fips { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("absentee_votes")]
        public int AbsenteeVotes { get; set; }

        [JsonPropertyName("reporting")]
        public int Reporting { get; set; }

        [JsonPropertyName("precincts")]
        public int Precincts { get; set; }

        [JsonPropertyName("absentee_method")]
        public string AbsenteeMethod { get; set; }

        [JsonPropertyName("eevp")]
        public int Eevp { get; set; }

        [JsonPropertyName("tot_exp_vote")]
        public int TotExpVote { get; set; }

        [JsonPropertyName("eevp_value")]
        public string EevpValue { get; set; }

        [JsonPropertyName("eevp_display")]
        public string EevpDisplay { get; set; }

        [JsonPropertyName("eevp_source")]
        public string EevpSource { get; set; }

        [JsonPropertyName("turnout_stage")]
        public int TurnoutStage { get; set; }

        [JsonPropertyName("absentee_count_progress")]
        public string AbsenteeCountProgress { get; set; }

        [JsonPropertyName("absentee_outstanding")]
        public object AbsenteeOutstanding { get; set; }

        [JsonPropertyName("absentee_max_ballots")]
        public int AbsenteeMaxBallots { get; set; }

        [JsonPropertyName("provisional_outstanding")]
        public object ProvisionalOutstanding { get; set; }

        [JsonPropertyName("provisional_count_progress")]
        public object ProvisionalCountProgress { get; set; }

        [JsonPropertyName("results")]
        public Results Results { get; set; }

        [JsonPropertyName("results_absentee")]
        public ResultsAbsentee ResultsAbsentee { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("leader_margin_value")]
        public double LeaderMarginValue { get; set; }

        [JsonPropertyName("leader_margin_display")]
        public string LeaderMarginDisplay { get; set; }

        [JsonPropertyName("leader_margin_name_display")]
        public string LeaderMarginNameDisplay { get; set; }

        [JsonPropertyName("leader_party_id")]
        public string LeaderPartyId { get; set; }

        [JsonPropertyName("margin2020")]
        public double Margin2020 { get; set; }

        [JsonPropertyName("votes2016")]
        public int Votes2016 { get; set; }

        [JsonPropertyName("margin2016")]
        public double Margin2016 { get; set; }

        [JsonPropertyName("votes2012")]
        public int Votes2012 { get; set; }

        [JsonPropertyName("margin2012")]
        public double Margin2012 { get; set; }
    }
    public class PrecinctMetadata
    {
        [JsonPropertyName("latest_url")]
        public string LatestUrl { get; set; }

        [JsonPropertyName("timestamped_url")]
        public string TimestampedUrl { get; set; }

        [JsonPropertyName("last_run")]
        public string LastRun { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
    public class TopLevel
    {
        [JsonPropertyName("sentence")]
        public string Sentence { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("is_new")]
        public bool IsNew { get; set; }

        [JsonPropertyName("hide_timestamp")]
        public bool HideTimestamp { get; set; }

        [JsonPropertyName("overrideText")]
        public object OverrideText { get; set; }

        [JsonPropertyName("generatedText")]
        public string GeneratedText { get; set; }

        [JsonPropertyName("sentence_type")]
        public string SentenceType { get; set; }
    }
    public class WinnerCardLeadin
    {
        [JsonPropertyName("sentence")]
        public string Sentence { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("is_new")]
        public bool IsNew { get; set; }

        [JsonPropertyName("hide_timestamp")]
        public bool HideTimestamp { get; set; }

        [JsonPropertyName("overrideText")]
        public object OverrideText { get; set; }

        [JsonPropertyName("generatedText")]
        public string GeneratedText { get; set; }

        [JsonPropertyName("sentence_type")]
        public string SentenceType { get; set; }
    }
    public class Counties
    {
        [JsonPropertyName("sentence")]
        public string Sentence { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("is_new")]
        public bool IsNew { get; set; }

        [JsonPropertyName("hide_timestamp")]
        public bool HideTimestamp { get; set; }

        [JsonPropertyName("overrideText")]
        public object OverrideText { get; set; }

        [JsonPropertyName("generatedText")]
        public string GeneratedText { get; set; }

        [JsonPropertyName("sentence_type")]
        public string SentenceType { get; set; }
    }
    public class Eevp
    {
        [JsonPropertyName("sentence")]
        public string Sentence { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("is_new")]
        public bool IsNew { get; set; }

        [JsonPropertyName("hide_timestamp")]
        public bool HideTimestamp { get; set; }

        [JsonPropertyName("overrideText")]
        public object OverrideText { get; set; }

        [JsonPropertyName("generatedText")]
        public string GeneratedText { get; set; }

        [JsonPropertyName("sentence_type")]
        public string SentenceType { get; set; }
    }
    public class EevpLeadin
    {
        [JsonPropertyName("sentence")]
        public string Sentence { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("is_new")]
        public bool IsNew { get; set; }

        [JsonPropertyName("hide_timestamp")]
        public bool HideTimestamp { get; set; }

        [JsonPropertyName("overrideText")]
        public object OverrideText { get; set; }

        [JsonPropertyName("generatedText")]
        public string GeneratedText { get; set; }

        [JsonPropertyName("sentence_type")]
        public string SentenceType { get; set; }
    }
    public class UpdateSentences
    {
        [JsonPropertyName("top_level")]
        public TopLevel TopLevel { get; set; }

        [JsonPropertyName("winner_card_leadin")]
        public WinnerCardLeadin WinnerCardLeadin { get; set; }

        [JsonPropertyName("counties")]
        public Counties Counties { get; set; }

        [JsonPropertyName("eevp")]
        public Eevp Eevp { get; set; }

        [JsonPropertyName("eevp_leadin")]
        public EevpLeadin EevpLeadin { get; set; }
    }
    public class BooleanThingsThatHappened
    {
        [JsonPropertyName("zero_votes")]
        public bool ZeroVotes { get; set; }

        [JsonPropertyName("results_expected_within_hour")]
        public bool ResultsExpectedWithinHour { get; set; }

        [JsonPropertyName("show_nothing_votes_decreased")]
        public bool ShowNothingVotesDecreased { get; set; }

        [JsonPropertyName("went_to_runoff")]
        public bool WentToRunoff { get; set; }

        [JsonPropertyName("race_won")]
        public bool RaceWon { get; set; }

        [JsonPropertyName("race_just_won")]
        public bool RaceJustWon { get; set; }

        [JsonPropertyName("race_unwon")]
        public bool RaceUnwon { get; set; }

        [JsonPropertyName("additional_votes_reported")]
        public bool AdditionalVotesReported { get; set; }

        [JsonPropertyName("additional_precincts_reported")]
        public bool AdditionalPrecinctsReported { get; set; }

        [JsonPropertyName("has_eevp")]
        public bool HasEevp { get; set; }
    }
    public class DetailsAboutChanges
    {
    }
    public class RaceDiff
    {
        [JsonPropertyName("race_slug")]
        public string RaceSlug { get; set; }

        [JsonPropertyName("boolean_things_that_happened")]
        public BooleanThingsThatHappened BooleanThingsThatHappened { get; set; }

        [JsonPropertyName("details_about_changes")]
        public DetailsAboutChanges DetailsAboutChanges { get; set; }
    }
    public class ModelMetadata
    {
        [JsonPropertyName("latest_url")]
        public string LatestUrl { get; set; }

        [JsonPropertyName("preview_latest_url")]
        public string PreviewLatestUrl { get; set; }

        [JsonPropertyName("hide_model")]
        public string HideModel { get; set; }

        [JsonPropertyName("model_note")]
        public string ModelNote { get; set; }

        [JsonPropertyName("candidates")]
        public List<string> Candidates { get; set; }
    }
    public class VoteShares
    {
        [JsonPropertyName("trumpd")]
        public double Trumpd { get; set; }

        [JsonPropertyName("bidenj")]
        public double Bidenj { get; set; }
    }
    public class Timesery
    {
        [JsonPropertyName("vote_shares")]
        public VoteShares VoteShares { get; set; } = new VoteShares();

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("eevp")]
        public int Eevp { get; set; }

        [JsonPropertyName("eevp_source")]
        public string EevpSource { get; set; } = "not filled in";

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
    public class Race
    {
        [JsonPropertyName("race_id")]
        public string RaceId { get; set; }

        [JsonPropertyName("race_slug")]
        public string RaceSlug { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("state_page_url")]
        public string StatePageUrl { get; set; }

        [JsonPropertyName("ap_polls_page")]
        public string ApPollsPage { get; set; }

        [JsonPropertyName("edison_exit_polls_page")]
        public string EdisonExitPollsPage { get; set; }

        [JsonPropertyName("race_type")]
        public string RaceType { get; set; }

        [JsonPropertyName("election_type")]
        public string ElectionType { get; set; }

        [JsonPropertyName("election_date")]
        public string ElectionDate { get; set; }

        [JsonPropertyName("runoff")]
        public bool Runoff { get; set; }

        [JsonPropertyName("race_name")]
        public string RaceName { get; set; }

        [JsonPropertyName("office")]
        public string Office { get; set; }

        [JsonPropertyName("officeid")]
        public string Officeid { get; set; }

        [JsonPropertyName("nyt_race_description")]
        public string NytRaceDescription { get; set; }

        [JsonPropertyName("race_rating")]
        public string RaceRating { get; set; }

        [JsonPropertyName("seat")]
        public string Seat { get; set; }

        [JsonPropertyName("seat_name")]
        public string SeatName { get; set; }

        [JsonPropertyName("state_id")]
        public string StateId { get; set; }

        [JsonPropertyName("state_slug")]
        public string StateSlug { get; set; }

        [JsonPropertyName("state_name")]
        public string StateName { get; set; }

        [JsonPropertyName("state_nyt_abbrev")]
        public string StateNytAbbrev { get; set; }

        [JsonPropertyName("state_shape")]
        public string StateShape { get; set; }

        [JsonPropertyName("party_id")]
        public string PartyId { get; set; }

        [JsonPropertyName("uncontested")]
        public bool Uncontested { get; set; }

        [JsonPropertyName("report")]
        public bool Report { get; set; }

        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("result_source")]
        public string ResultSource { get; set; }

        [JsonPropertyName("gain")]
        public bool Gain { get; set; }

        [JsonPropertyName("lost_seat")]
        public string LostSeat { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("electoral_votes")]
        public int ElectoralVotes { get; set; }

        [JsonPropertyName("absentee_votes")]
        public int AbsenteeVotes { get; set; }

        [JsonPropertyName("absentee_counties")]
        public int AbsenteeCounties { get; set; }

        [JsonPropertyName("absentee_count_progress")]
        public string AbsenteeCountProgress { get; set; }

        [JsonPropertyName("absentee_outstanding")]
        public object AbsenteeOutstanding { get; set; }

        [JsonPropertyName("absentee_max_ballots")]
        public object AbsenteeMaxBallots { get; set; }

        [JsonPropertyName("provisional_outstanding")]
        public object ProvisionalOutstanding { get; set; }

        [JsonPropertyName("provisional_count_progress")]
        public object ProvisionalCountProgress { get; set; }

        [JsonPropertyName("poll_display")]
        public string PollDisplay { get; set; }

        [JsonPropertyName("poll_countdown_display")]
        public string PollCountdownDisplay { get; set; }

        [JsonPropertyName("poll_waiting_display")]
        public string PollWaitingDisplay { get; set; }

        [JsonPropertyName("poll_time")]
        public DateTime PollTime { get; set; }

        [JsonPropertyName("poll_time_short")]
        public string PollTimeShort { get; set; }

        [JsonPropertyName("precincts_reporting")]
        public int PrecinctsReporting { get; set; }

        [JsonPropertyName("precincts_total")]
        public int PrecinctsTotal { get; set; }

        [JsonPropertyName("reporting_display")]
        public string ReportingDisplay { get; set; }

        [JsonPropertyName("reporting_value")]
        public string ReportingValue { get; set; }

        [JsonPropertyName("eevp")]
        public int Eevp { get; set; }

        [JsonPropertyName("tot_exp_vote")]
        public int TotExpVote { get; set; }

        [JsonPropertyName("eevp_source")]
        public string EevpSource { get; set; }

        [JsonPropertyName("eevp_value")]
        public string EevpValue { get; set; }

        [JsonPropertyName("eevp_display")]
        public string EevpDisplay { get; set; }

        [JsonPropertyName("county_data_source")]
        public string CountyDataSource { get; set; }

        [JsonPropertyName("incumbent_party")]
        public string IncumbentParty { get; set; }

        [JsonPropertyName("no_forecast")]
        public bool NoForecast { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("candidates")]
        public List<Candidate> Candidates { get; set; }

        [JsonPropertyName("has_incumbent")]
        public bool HasIncumbent { get; set; }

        [JsonPropertyName("leader_margin_value")]
        public double LeaderMarginValue { get; set; }

        [JsonPropertyName("leader_margin_votes")]
        public int LeaderMarginVotes { get; set; }

        [JsonPropertyName("leader_margin_display")]
        public string LeaderMarginDisplay { get; set; }

        [JsonPropertyName("leader_margin_name_display")]
        public string LeaderMarginNameDisplay { get; set; }

        [JsonPropertyName("leader_party_id")]
        public string LeaderPartyId { get; set; }

        [JsonPropertyName("counties")]
        public List<County> Counties { get; set; }

        [JsonPropertyName("precinct_metadata")]
        public PrecinctMetadata PrecinctMetadata { get; set; }

        [JsonPropertyName("votes2016")]
        public int Votes2016 { get; set; }

        [JsonPropertyName("margin2016")]
        public double Margin2016 { get; set; }

        [JsonPropertyName("clinton2016")]
        public int Clinton2016 { get; set; }

        [JsonPropertyName("trump2016")]
        public int Trump2016 { get; set; }

        [JsonPropertyName("votes2012")]
        public int Votes2012 { get; set; }

        [JsonPropertyName("margin2012")]
        public double Margin2012 { get; set; }

        [JsonPropertyName("expectations_text")]
        public string ExpectationsText { get; set; }

        [JsonPropertyName("expectations_text_short")]
        public string ExpectationsTextShort { get; set; }

        [JsonPropertyName("absentee_ballot_deadline")]
        public int AbsenteeBallotDeadline { get; set; }

        [JsonPropertyName("absentee_postmark_deadline")]
        public int AbsenteePostmarkDeadline { get; set; }

        [JsonPropertyName("update_sentences")]
        public UpdateSentences UpdateSentences { get; set; }

        [JsonPropertyName("race_diff")]
        public RaceDiff RaceDiff { get; set; }

        [JsonPropertyName("winnerCalledTimestamp")]
        public long WinnerCalledTimestamp { get; set; }

        [JsonPropertyName("model_metadata")]
        public ModelMetadata ModelMetadata { get; set; }

        [JsonPropertyName("timeseries")]
        public List<Timesery> TimeSeries { get; set; } = new List<Timesery>();
    }
    
    // Data - Child...
    public class Data
    {
        [JsonPropertyName("races")]
        public List<Race> Races { get; set; }
        [JsonPropertyName("party_control")]
        public List<PartyControl> PartyControl { get; set; }
        [JsonPropertyName("liveUpdates")]
        public List<LiveUpdate> LiveUpdates { get; set; }
    }

    // Meta - Child...
    public class Meta
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("track")]
        public string Track { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    // ROOT...
    public class Root
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }
    }
}
