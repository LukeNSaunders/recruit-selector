namespace Server.Models;

public class RecruitResult
{
    public List<int> SelectedRecruits { get; set; } = new List<int>();

    public int FinalRecruit { get; set; }
}