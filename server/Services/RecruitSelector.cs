using Server.Models;

namespace Server.Services;

public class RecruitSelector
{
    public RecruitResult SelectRecruits(int n, int k)
    {
        // Make a list of recruits up to n
        var circle = new List<int>();
        for (int i = 1; i <= n; i++)
        {
            circle.Add(i);
        }

        // The list will hold recruits in the order they get picked
        var selected = new List<int>();

        // Sergeant starts at the first recruit
        int position = 0;

        // Repeat until only one recruit is left
        while (circle.Count > 1)
        {
            // Move forward k recruits.
            // If we go past the end, % takes us back to the start.
            position = (position + k) % circle.Count;

            // Recruit gets picked and taken out of the circle
            selected.Add(circle[position]);
            circle.RemoveAt(position);

            // Everyone after them moves up one spot,
            // so 'position' is already on the next recruit
        }

        // return back who was picked, and who is left
        return new RecruitResult
        {
            SelectedRecruits = selected,
            FinalRecruit = circle[0]
        };
    }
}