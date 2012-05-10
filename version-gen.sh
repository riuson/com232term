#/bin/sh
git log --pretty=format:"git-commit-info %h %ad" -1 > version-included.txt