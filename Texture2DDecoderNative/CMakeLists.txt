cmake_minimum_required(VERSION 3.19.0)

project(Texture2DDecoderNative)

set(TEXTURE2DDECODER_SOURCES
    "astc.cpp"    
    "atc.cpp"    
    "bcn.cpp"    
    "crunch.cpp"    
    "dllmain.cpp"    
    "etc.cpp"    
    "pvrtc.cpp"    
    "unitycrunch.cpp"    
)

add_library(Texture2DDecoderNative SHARED ${TEXTURE2DDECODER_SOURCES})

target_include_directories(Texture2DDecoderNative PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})
target_compile_definitions(Texture2DDecoderNative PUBLIC _T2D_DLL TEXTURE2DDECODERNATIVE_EXPORTS _USRDLL)

if (CMAKE_BUILD_TYPE MATCHES "Debug")
    target_compile_definitions(Texture2DDecoderNative PUBLIC _DEBUG)
else()
    target_compile_definitions(Texture2DDecoderNative PUBLIC NDEBUG)
endif ()

if (NOT MSVC)
    target_compile_options(Texture2DDecoderNative PUBLIC -Wno-c++11-narrowing -Wno-unused-value)
endif ()