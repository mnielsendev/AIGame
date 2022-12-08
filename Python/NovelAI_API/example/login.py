import sys
from sys import path
from os.path import join, abspath, dirname

path.insert(0, abspath(join(dirname(sys.argv[0]), '..')))

from novelai_api import NovelAI_API
from aiohttp import ClientSession

from logging import Logger, StreamHandler
from asyncio import run

username = 'redacted'
password = 'redacted'

logger = Logger('NovelAI')
logger.addHandler(StreamHandler())

async def main():
    async with ClientSession() as session:
        api = NovelAI_API(session, logger = logger)
        logger.info(await api.high_level.login(username, password))

        if await api.low_level.is_reachable():
            print('NovelAI Login Successful')

run(main())