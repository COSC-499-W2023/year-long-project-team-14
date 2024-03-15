#!/bin/bash

# Iterate over each file tracked by Git LFS and untrack it
git lfs ls-files | cut -d ' ' -f 3 | while read -r file; do
    git lfs untrack "$file"
done
