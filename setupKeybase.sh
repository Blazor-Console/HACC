#!/bin/bash
hasRemote() {
    status=$(git remote show | grep "keybase" | wc -l)
    return [[ status -eq 1 ]];
}

setupRemote() {
    if [[ ! hasRemote ]]; then
        git remote add keybase "$1"
    fi
}

# make sure we're at toplevel
SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" &> /dev/null && pwd)
OPWD=$(pwd)
cd "${SCRIPT_DIR}"
setupRemote keybase://team/digitalrebellion/HACC.Development
cd "${SCRIPT_DIR}/src/HACC"
setupRemote keybase://team/digitalrebellion/HACC
cd "${SCRIPT_DIR}/src/HACC.Demo"
setupRemote keybase://team/digitalrebellion/HACC.Demo
cd "${SCRIPT_DIR}/test/HACC.Tests"
setupRemote keybase://team/digitalrebellion/HACC.Tests
cd "${OPWD}"
