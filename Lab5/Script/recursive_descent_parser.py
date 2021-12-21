import sys

EOI = 0
NUM2 = 1.2
BIN_DET = '#'
NUM8 = 1.8
OCT_DET = '"'
NUM10 = 1.10
DEC_DET = '\''
VAR = 2
NEGATIVE = 3
ADDITIVE = 4
MULTI = 5
EQUAL = 6
LB = 20
RB = 21
SEPARATOR = 13
UNKNOWN = -1

TOKEN = 0
LEXEME = 1

SUCCESS = 0
ERROR = -1

MATCHING_DICT = {
    NUM2: ['#', '0', '1'],
    NUM8: ['"', '0', '1', '2', '3', '4',
           '5', '6', '7'],
    NUM10: ['\'', '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9'],
    VAR: ['a', 'b', 'c', 'd', 'e', 'f'],
    NEGATIVE: ['!'],
    ADDITIVE: ['+', '-'],
    MULTI: ['*', '/', '%'],
    LB: ['('],
    RB: [')'],
    EQUAL: ['='],
    SEPARATOR: [';'],
    EOI: ['$']
}


class RDParser:
    input_index = 0
    str_for_parse = ''
    stack = 'S'

    @staticmethod
    def get_next_token():
        temp_token = EOI

        if RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[NUM2]:
            temp_token = NUM2
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[NUM8]:
            temp_token = NUM8
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[NUM10]:
            temp_token = NUM10
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[VAR]:
            temp_token = VAR
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[LB]:
            temp_token = LB
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[RB]:
            temp_token = RB
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[NEGATIVE]:
            temp_token = NEGATIVE
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[MULTI]:
            temp_token = MULTI
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[ADDITIVE]:
            temp_token = ADDITIVE
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[EQUAL]:
            temp_token = EQUAL
        elif RDParser.str_for_parse[RDParser.input_index] in MATCHING_DICT[SEPARATOR]:
            temp_token = SEPARATOR
        elif len(RDParser.str_for_parse) > RDParser.input_index + 1:
            temp_token = UNKNOWN

        if temp_token == UNKNOWN:
            RDParser.raise_error(Exception('Unknown input symbol!'))

        if temp_token != EOI:
            RDParser.input_index += 1
            return temp_token, RDParser.str_for_parse[RDParser.input_index - 1]
        return temp_token, ''

    @staticmethod
    def token_rollback():
        RDParser.input_index -= 1

    @staticmethod
    def raise_error(exc=Exception('Wrong input!')):
        print('Rejected!')
        raise exc

    @staticmethod
    def parse(str_for_parse):

        RDParser.input_index = 0
        RDParser.stack = 'S'
        RDParser.str_for_parse = str_for_parse + '$'

        if RDParser.start() == 0 and len(str_for_parse) == RDParser.input_index + 1:
            print('Accepted!')
            return True
        else:
            print('Rejected!')
            return False

    @staticmethod
    def print_stack():
        print(RDParser.stack)

    @staticmethod
    def stack_update(func_name, production):
        RDParser.stack = RDParser.stack.replace(func_name, production, 1)

    @staticmethod
    def start():
        res = SUCCESS
        token = RDParser.get_next_token()
        RDParser.print_stack()

        if token[TOKEN] == VAR:
            RDParser.stack_update('S', token[LEXEME] + 'BCD;E')
            RDParser.print_stack()
            res += RDParser.b_func()
            res += RDParser.c_func()
            res += RDParser.d_func()
            if RDParser.get_next_token()[TOKEN] != SEPARATOR:
                RDParser.raise_error()
                # res += ERROR
            res += RDParser.e_func()
        else:
            RDParser.raise_error()
            # res += ERROR
        return res

    @staticmethod
    def b_func():
        token = RDParser.get_next_token()
        if token[TOKEN] == VAR:
            RDParser.stack_update('B', token[LEXEME])
            RDParser.print_stack()
            return SUCCESS
        else:
            RDParser.stack_update('B', '')
            RDParser.print_stack()
            RDParser.token_rollback()
            return SUCCESS

    @staticmethod
    def c_func():
        token = RDParser.get_next_token()
        if token[TOKEN] == EQUAL:
            RDParser.stack_update('C', '=')
            RDParser.print_stack()
            return SUCCESS
        else:
            RDParser.raise_error()
            # return ERROR

    @staticmethod
    def d_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[TOKEN] == LB:
            RDParser.stack_update('D', '{D}F')
            RDParser.print_stack()
            res += RDParser.d_func()
            if RDParser.get_next_token()[TOKEN] != RB:
                RDParser.raise_error()
                # res += ERROR
            res += RDParser.f_func()
        elif token[TOKEN] == VAR:
            RDParser.stack_update('D', token[LEXEME] + 'BF')
            RDParser.print_stack()
            res += RDParser.b_func()
            res += RDParser.f_func()
        elif token[TOKEN] == NUM2 and token[LEXEME] == BIN_DET:
            RDParser.stack_update('D', '#LMF')
            RDParser.print_stack()
            res += RDParser.l_func()
            res += RDParser.m_func()
            res += RDParser.f_func()
        elif token[TOKEN] == NUM8 and token[LEXEME] == OCT_DET:
            RDParser.stack_update('D', '"JKF')
            RDParser.print_stack()
            res += RDParser.j_func()
            res += RDParser.k_func()
            res += RDParser.f_func()
        elif token[TOKEN] == NUM10 and token[LEXEME] == DEC_DET:
            RDParser.stack_update('D', '\'GIF')
            RDParser.print_stack()
            res += RDParser.g_func()
            res += RDParser.i_func()
            res += RDParser.f_func()
        elif token[TOKEN] == NEGATIVE:
            RDParser.stack_update('D', '-D')
            RDParser.print_stack()
            res += RDParser.d_func()
        else:
            RDParser.raise_error()
            # res += ERROR
        return res

    @staticmethod
    def e_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[TOKEN] == VAR:
            RDParser.stack_update('E', token[LEXEME] + 'BCD;E')
            RDParser.print_stack()
            res += RDParser.b_func()
            res += RDParser.c_func()
            res += RDParser.d_func()
            if RDParser.get_next_token()[TOKEN] != SEPARATOR:
                RDParser.raise_error()
                # res += ERROR
            res += RDParser.e_func()
        else:
            RDParser.stack_update('E', '')
            RDParser.print_stack()
            RDParser.token_rollback()
        return res

    @staticmethod
    def f_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[TOKEN] == MULTI:
            RDParser.stack_update('F', token[LEXEME] + 'D')
            RDParser.print_stack()
            res += RDParser.d_func()
        elif token[TOKEN] == ADDITIVE:
            RDParser.stack_update('F', 'H')
            RDParser.print_stack()
            RDParser.token_rollback()
            res += RDParser.h_func()
        else:
            RDParser.stack_update('F', '')
            RDParser.print_stack()
            RDParser.token_rollback()
        return res

    @staticmethod
    def g_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[LEXEME] in MATCHING_DICT[NUM10]:
            RDParser.stack_update('G', token[LEXEME])
            RDParser.print_stack()
        else:
            RDParser.raise_error()
        return res

    @staticmethod
    def h_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[TOKEN] == ADDITIVE:
            RDParser.stack_update('H', token[LEXEME] + 'D')
            RDParser.print_stack()
            res += RDParser.d_func()
        else:
            RDParser.raise_error()
        return res

    @staticmethod
    def i_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[LEXEME] in MATCHING_DICT[NUM10]:
            RDParser.stack_update('I', token[LEXEME] + 'I')
            RDParser.print_stack()
            res += RDParser.i_func()
        else:
            RDParser.stack_update('I', '')
            RDParser.print_stack()
            RDParser.token_rollback()
        return res

    @staticmethod
    def j_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[LEXEME] in MATCHING_DICT[NUM8]:
            RDParser.stack_update('J', token[LEXEME])
            RDParser.print_stack()
        else:
            RDParser.raise_error()
        return res

    @staticmethod
    def k_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[LEXEME] in MATCHING_DICT[NUM8]:
            RDParser.stack_update('K', token[LEXEME] + 'K')
            RDParser.print_stack()
            res += RDParser.k_func()
        else:
            RDParser.stack_update('K', '')
            RDParser.print_stack()
            RDParser.token_rollback()
        return res

    @staticmethod
    def l_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[LEXEME] in MATCHING_DICT[NUM2]:
            RDParser.stack_update('L', token[LEXEME])
            RDParser.print_stack()
        else:
            RDParser.raise_error()
        return res

    @staticmethod
    def m_func():
        res = SUCCESS
        token = RDParser.get_next_token()
        if token[LEXEME] in MATCHING_DICT[NUM2]:
            RDParser.stack_update('M', token[LEXEME] + 'M')
            RDParser.print_stack()
            res += RDParser.m_func()
        else:
            RDParser.stack_update('M', '')
            RDParser.print_stack()
            RDParser.token_rollback()
        return res


def main():
    if len(sys.argv) > 1:
        try:
            RDParser.parse(sys.argv[1])
        except Exception as e:
            print(e)
    else:
        temp = input()
        try:
            RDParser.parse(temp)
        except Exception as e:
            print(e)


if __name__ == "__main__":
    main()
