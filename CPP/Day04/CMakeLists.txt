cmake_minimum_required(VERSION 3.24)
project(Day04)

set(CMAKE_CXX_STANDARD 20)

add_executable(Day04 main.cpp Assignment.cpp Assignment.h)

configure_file (input.txt input.txt COPYONLY)