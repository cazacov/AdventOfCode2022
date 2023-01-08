#include <iostream>
#include <fstream>
#include <regex>
#include <string>
#include <vector>
#include "Command.h"

std::pair<std::vector<std::vector<char>>, std::vector<Command>> read_input(std::string file_name);
void puzzle1(std::vector<std::vector<char>> crates,
             std::vector<Command> &commands);
void puzzle2(std::vector<std::vector<char>> crates,
             std::vector<Command> &commands);

int main() {
    std::cout << "Advent of Code 2022, day 5" << std::endl;

    auto [crates, commands] = read_input("input.txt");

    puzzle1(crates, commands);
    puzzle2(crates, commands);

    return 0;
}

void puzzle1(std::vector<std::vector<char>> crates, std::vector<Command> &commands) {

    for (auto &command : commands) {
        auto &from_stack = crates[command.from];
        auto &to_stack = crates[command.to];

        to_stack.insert(
                to_stack.end(),
                from_stack.rbegin(),
                from_stack.rbegin() + command.quantity);

        from_stack.resize(from_stack.size() - command.quantity);
    }

    std::cout << "Puzzle 1: ";
    for (auto &stack : crates) {
        std::cout << stack.back();
    }
    std::cout << std::endl;
}

void puzzle2(std::vector<std::vector<char>> crates, std::vector<Command> &commands) {
    for (auto &command : commands) {
        auto &from_stack = crates[command.from];
        auto &to_stack = crates[command.to];

        to_stack.insert(
                to_stack.end(),
                from_stack.begin() + (from_stack.size() - command.quantity),
                from_stack.end());

        from_stack.resize(from_stack.size() - command.quantity);
    }

    std::cout << "Puzzle 2: ";
    for (auto &stack : crates) {
        std::cout << stack.back();
    }
    std::cout << std::endl;

}

std::pair<std::vector<std::vector<char>>, std::vector<Command>> read_input(std::string file_name) {

    std::ifstream input { file_name};

    std::vector<std::vector<char>> crates;

    std::string line;
    while (std::getline(input, line)) {
        // Check footer (line contains no '[' character)
        if (line.find('[') == std::string::npos) {
            break;
        }
        for (auto i = 1; i < line.length(); i += 4) {
            auto stack_index = i / 4;
            // Add new crates stack if necessary
            if (crates.size() <= stack_index) {
                crates.resize(stack_index + 1);
            }
            // Add new crate to the stack
            if (isalnum(line[i])) {
                crates[stack_index].push_back(line[i]);
            }
        }
    }
    for (auto &crate : crates) {
        std::reverse(crate.begin(), crate.end());
    }

    // Skip empty lines
    while (std::getline(input, line)) {
        if (!line.empty()) {
            break;
        }
    }

    std::vector<Command> commands;

    std::regex command_regex("move (\\d+) from (\\d+) to (\\d+)");
    do {
        std::smatch groups;
        auto match = std::regex_match(line, groups, command_regex);
        if (match) {
            int from = std::stoi(groups[2]);
            int to = std::stoi(groups[3]);
            int quantity = std::stoi(groups[1]);
            commands.push_back(Command(
                    from - 1, // Crates indexing is zero-based
                    to - 1,
                    quantity));
        }
    } while (std::getline(input, line));

    return std::pair<std::vector<std::vector<char>>, std::vector<Command>> {
            crates,
            commands
    };
}
