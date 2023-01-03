#include <fstream>
#include <iostream>
#include <vector>
#include <ranges>
#include <algorithm>
#include "Assignment.h"

void puzzle1(std::vector<std::pair<Assignment, Assignment>> &pairs);
void puzzle2(std::vector<std::pair<Assignment, Assignment>> &pairs);
std::vector<std::pair<Assignment, Assignment>> read_input(std::string file_name);

int main() {
    std::cout << "Advent of Code 2022, day 4" << std::endl;

    auto pairs = read_input("input.txt");

    puzzle1(pairs);
    puzzle2(pairs);
}

void puzzle1(std::vector<std::pair<Assignment, Assignment>> &pairs) {
    auto result = std::count_if(pairs.begin(), pairs.end(), [](const auto &pair) {
        auto intersection = pair.first & pair.second;
        return
            intersection == pair.first
            || intersection == pair.second;
    });

    std::cout << "Puzzle 1: " << result << std::endl;
}

void puzzle2(std::vector<std::pair<Assignment, Assignment>> &pairs) {
    auto puzzle2 = std::count_if(pairs.begin(), pairs.end(), [](const auto &pair) {
        auto intersection = pair.first & pair.second;
        return !intersection.empty();
    });

    std::cout << "Puzzle 2: " << puzzle2 << std::endl;
}

std::vector<std::pair<Assignment, Assignment>> read_input(std::string file_name) {

    std::vector<std::pair<Assignment, Assignment>> result;

    auto to_assignments = [] (std::string &line) -> std::pair<Assignment, Assignment> {
        auto comma = line.find(',');
        return  {
                line.substr(0, comma),
                line.substr(comma + 1)
        };
    };

    std::ifstream input { file_name};
    auto assignment =
            std::ranges::istream_view<std::string>(input)
            | std::views::transform(to_assignments);

    std::ranges::copy(assignment, std::back_inserter(result));
    return result;
}