cmake_minimum_required(VERSION 3.19)

project(AssetStudio)

set(CMAKE_CXX_STANDARD 17)

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/bin)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/bin)

set(CSBUILD_PROJECT_DIR ${CMAKE_CURRENT_SOURCE_DIR}/csproj)

add_subdirectory(AssetStudioCLI)
add_subdirectory(Texture2DDecoderNative)

