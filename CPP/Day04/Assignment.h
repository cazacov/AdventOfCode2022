//
// Created by Victor on 03.01.2023.
//

#ifndef DAY04_ASSIGNMENT_H
#define DAY04_ASSIGNMENT_H

#include <string>
#include <charconv>

class Assignment {
private:
    int from;
    int to;
public:
    Assignment (int from_, int to_) : from{from_}, to{to_}
    {
    }

    Assignment (std::string range) {
        auto dash = range.find('-');
        from =  std::stoi(range.substr(0, dash));
        to =  std::stoi(range.substr(dash+1));
    }

    bool empty() {
        return from > to;
    }

    Assignment operator&(const Assignment &rhs) const {

        auto newFrom = std::max(from, rhs.from);
        auto newTo = std::min(to, rhs.to);

        if (newTo < newFrom) {
            return Assignment(0, -1);
        }
        else {
            return Assignment(newFrom, newTo);
        }
    }

    bool operator==(const Assignment &rhs) const {
        return from == rhs.from &&
               to == rhs.to;
    }

    bool operator!=(const Assignment &rhs) const {
        return !(rhs == *this);
    }
};


#endif //DAY04_ASSIGNMENT_H
