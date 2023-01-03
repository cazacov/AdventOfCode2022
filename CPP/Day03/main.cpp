#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <algorithm>
#include <numeric>
#include <set>
#include <unordered_set>

using namespace std;

vector<string> read_input(std::string file_name);
void puzzle1(vector<string> &input);
void puzzle2(vector<string> &input);

int main() {
    std::cout << "Advent of Code 2022, day 3" << std::endl;

    auto input = read_input("input.txt");

    puzzle1(input);
    puzzle2(input);
}

vector<string> read_input(std::string file_name) {

    ifstream input { file_name};

    vector<string> result;

    string line;
    while (getline(input, line)) {
        result.push_back(line);
    }
    return result;
}

int item_score(const char &ch) {
    return (ch < 'a' ? ch - 'A' + 27 : ch - 'a' + 1);
}

void puzzle1(vector<string> &input) {

    int result = 0;

    for ( auto line : input) {
        auto first = line.substr(0, line.length() / 2);
        auto second = line.substr(line.length() / 2);

        set<char> first_set;
        for (auto &ch : first) {
            first_set.insert(ch);
        }

        set<char> second_set;
        for (auto &ch : second) {
            second_set.insert(ch);
        }

        vector<char> common_items;
        set_intersection(
                first_set.begin(), first_set.end(),
                second_set.begin(), second_set.end(),
                back_inserter(common_items));

        result += std::accumulate(common_items.begin(), common_items.end(), 0,
                                  [](int acc, const char &ch) {
                                      return acc + item_score(ch);
                                  });
    }

    std::cout << "Puzzle 1: " << result << std::endl;
}

void puzzle2(vector<string> &input) {

    int result = 0;

    unordered_set<char> common_items;

    for (int i = 0; i < input.size(); i++) {
        auto &line = input[i];
        if (i % 3 == 0) {
            if (!common_items.empty())
            {
                result += item_score(*common_items.begin());
                common_items.clear();
            }
            for (auto &ch : line) {
                common_items.insert(ch);
            }
        } else {
            std::erase_if(common_items, [common_items, line] (const auto &ch)
            {
                return line.find(ch) == string::npos;
            });
        }
    }
    result += item_score(*common_items.begin());

    std::cout << "Puzzle 2: " << result << std::endl;
}
