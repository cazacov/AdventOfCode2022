cmake_minimum_required(VERSION 3.24)
project(Day02)

set(CMAKE_CXX_STANDARD 20)

add_executable(Day02 main.cpp)

configure_file (input.txt input.txt COPYONLY)