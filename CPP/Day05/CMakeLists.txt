cmake_minimum_required(VERSION 3.24)
project(Day05)

set(CMAKE_CXX_STANDARD 20)

add_executable(Day05 main.cpp Command.cpp Command.h)

configure_file (input.txt input.txt COPYONLY)