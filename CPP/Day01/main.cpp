#include <iostream>
#include <fstream>
#include <vector>
#include <numeric>
#include <algorithm>
#include <string>


std::vector<std::vector<int>> readInput(const std::string& fileName) {

    std::vector<std::vector<int>> result;

    std::ifstream input;
    input.open(fileName, std::ios::in);

    std::vector<int> accumulator;
    while (!input.eof()) {
        std::string line;
        std::getline(input, line);

        if (line.empty()) {
            auto inventory = accumulator;
            result.push_back(inventory);
            accumulator.clear();
        }
        else {
            accumulator.push_back(std::stoi(line));
        }
    }
    if (!accumulator.empty()) {
        result.push_back(accumulator);
    }

    return result;
}


void puzzle1(std::vector<std::vector<int>> &inventories) {

    std::vector<int> sums;
    std::transform(inventories.begin(), inventories.end(), std::back_inserter(sums),
                   [](std::vector<int> &elem) {
                       return std::accumulate(elem.begin(), elem.end(), 0);
                   }
    );

    int maxSum = *std::max_element(sums.begin(), sums.end());

    std::cout << "Puzzle 1: " << maxSum << std::endl;
}

void puzzle2(std::vector<std::vector<int>> &inventories) {

    std::vector<int> sums;
    std::transform(inventories.begin(), inventories.end(), std::back_inserter(sums),
                   [](std::vector<int> &elem) {
                       return std::accumulate(elem.begin(), elem.end(), 0);
                   }
    );



    int first {0};
    int second {0};
    int third {0};

    for (auto &elem: sums) {
        if (elem > first) {
            third = second;
            second = first;
            first = elem;
        }
        else if (elem > second) {
            third = second;
            second = elem;
        }
        else if (elem > third) {
            third = elem;
        }
    }

    std::cout << "Puzzle 2: " << first + second + third  << std::endl;
}

int main() {
    std::cout << "Advent of Code 2022, day 1" << std::endl;

    auto input = readInput("input.txt");

    puzzle1(input);
    puzzle2(input);
    return 0;
}