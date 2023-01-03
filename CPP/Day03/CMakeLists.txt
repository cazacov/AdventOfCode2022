cmake_minimum_required(VERSION 3.24)
project(Day03)

set(CMAKE_CXX_STANDARD 20)

add_executable(Day03 main.cpp)

configure_file (input.txt input.txt COPYONLY)