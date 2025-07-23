#!/bin/bash

cd "$(dirname "$0")"
cd frontend || {
  echo "Folder 'frontend' not found!"
  exit 1
}

case "$1" in
--dev)
  echo "Starting frontend in development mode..."
  npm install
  npm run dev
  ;;
--build)
  echo "Building frontend project..."
  npm install
  npm run build
  ;;
*)
  echo "Usage: $0 [--dev | --build]"
  exit 1
  ;;
esac
