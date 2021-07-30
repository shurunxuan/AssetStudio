cmake_minimum_required(VERSION 3.19)

project(AssetStudioCLI)

list(APPEND CMAKE_MODULE_PATH "${CMAKE_SOURCE_DIR}/cmake")
find_package(DotNetStandard)

set(CSBUILD_AssetStudioCLI_CSPROJ "${CMAKE_CURRENT_SOURCE_DIR}/AssetStudioCLI.csproj")
file(TO_NATIVE_PATH ${CSBUILD_AssetStudioCLI_CSPROJ} CSBUILD_AssetStudioCLI_CSPROJ)

message (STATUS "Restore packages")
execute_process(COMMAND ${DOTNET_STANDARD_EXECUTABLE} restore ${CSBUILD_AssetStudioCLI_CSPROJ})

if (CMAKE_BUILD_TYPE MATCHES "Debug")
    set(CSHARP_CONFIGURATION "Debug")
else ()
    set(CSHARP_CONFIGURATION "Release")
endif ()


if (WIN32)
    set(CSHARP_OS "win")
elseif (APPLE)
    set(CSHARP_OS "osx")
elseif (UNIX)
    set(CSHARP_OS "linux")
else()
    set(CSHARP_OS "unknown")
endif ()

string( TOLOWER "${CMAKE_HOST_SYSTEM_PROCESSOR}" CSHARP_PROCESSOR )
if ( CSHARP_PROCESSOR MATCHES "arm")
    if( CMAKE_SIZEOF_VOID_P EQUAL 8 )
        set(CSHARP_ARCH "arm64")
    elseif(CMAKE_SIZEOF_VOID_P EQUAL 4)
        set(CSHARP_ARCH "arm")
    endif()
else()
    if( CMAKE_SIZEOF_VOID_P EQUAL 8 )
        set(CSHARP_ARCH "x64")
    elseif(CMAKE_SIZEOF_VOID_P EQUAL 4)
        set(CSHARP_ARCH "x86")
    endif()
endif()

set (CSHARP_RID "${CSHARP_OS}-${CSHARP_ARCH}")

message (STATUS "C# project building with ${CSHARP_CONFIGURATION} configuration targeting ${CSHARP_RID}")



add_custom_target(
    AssetStudioCLI ALL

    COMMAND ${DOTNET_STANDARD_EXECUTABLE} publish ${CSBUILD_AssetStudioCLI_CSPROJ} -r ${CSHARP_RID} -f net6.0 -o ${CMAKE_RUNTIME_OUTPUT_DIRECTORY} -c ${CSHARP_CONFIGURATION}
    COMMENT "Building ${CSBUILD_AssetStudioCLI_CSPROJ} with configuration ${CSHARP_CONFIGURATION} targeting ${CSHARP_RID}"
)

