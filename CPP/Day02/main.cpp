#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <ranges>
#include <numeric>

std::vector<std::pair<int, int>> read_input(std::string file_name) {

    std::ifstream input { file_name};

    std::vector<std::pair<int, int>> result;

    std::string line;
    while (std::getline(input, line)) {
        result.push_back(std::pair<int, int> {
            line[0] - 'A',
            line[2] - 'X'
        });
    }
    return result;
}

// The score for a single round
int game_score(int opponent_move, int my_move) {
    /*
     * @param opponent_move 0 - Rock, 1 - Paper, 2 - Scissors
     * @param my_move       0 - Rock, 1 - Paper, 2 - Scissors
     * @return              the score for a single round
     *
     * The score for a single round is the score for the shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors)
     * plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won).
     */

    return (my_move + 1)
        + ((4 + my_move - opponent_move) % 3) * 3;
}

void puzzle1(std::vector<std::pair<int, int>> &input) {

    auto scores = input | std::views::transform([](auto &record){
        auto opponent_move = record.first;
        auto my_move = record.second;
        return game_score(opponent_move, my_move);
    });

    auto result = std::reduce(scores.begin(), scores.end());

    std::cout << "Puzzle 1: " << result << std::endl;
}

void puzzle2(std::vector<std::pair<int, int>> &input) {

    auto scores = input | std::views::transform([](auto &record){
        auto opponent_move = record.first;
        auto my_move = (opponent_move + record.second + 2) % 3;
        return game_score(opponent_move, my_move);
    });

    auto result = std::reduce(scores.begin(), scores.end());

    std::cout << "Puzzle 2: " << result << std::endl;
}


int main() {
    std::cout << "Advent of Code 2022, day 2" << std::endl;

    auto input = read_input("input.txt");

    puzzle1(input);
    puzzle2(input);
}
