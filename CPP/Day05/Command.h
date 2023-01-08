//
// Created by Victor on 07.01.2023.
//

#ifndef DAY05_COMMAND_H
#define DAY05_COMMAND_H


class Command {
public:
    Command(int from, int to, int quantity);

public:
    int from;
    int to;
    int quantity;
};


#endif //DAY05_COMMAND_H
