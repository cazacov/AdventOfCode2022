cmake_minimum_required(VERSION 3.24)
project(Day01)

set(CMAKE_CXX_STANDARD 20)

add_executable(Day01 main.cpp)

configure_file (input.txt input.txt COPYONLY)