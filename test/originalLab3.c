#include "system.h"
#include "altera_up_avalon_accelerometer_spi.h"
#include "altera_avalon_timer_regs.h"
#include "altera_avalon_timer.h"
#include "altera_avalon_pio_regs.h"
#include "sys/alt_irq.h"
#include <stdlib.h>

#define OFFSET -32
#define PWM_PERIOD 16

alt_8 pwm = 0;
alt_u8 led;
//what is led? the value it spits out is 1 2 4 8 10 20 40 80 
//where 1 : LED0 and 80 : LED7
int level;
//this stays around 1e, 1d, 1f when the board doesn't move
//

void led_write(alt_u8 led_pattern) {
    IOWR(LED_BASE, 0, led_pattern);
    //we use from led 0 to led 7
}

void convert_read(alt_32 acc_read, int * level, alt_u8 * led) {
    acc_read += OFFSET; //you add -32 to the value? I guess this moves it so that the middle between positive and negative extremes is at -32 and not 0
    alt_u8 val = (acc_read >> 6) & 0x07; // val = accel data right shifted by 6 bits and ANDED with 0x07 = b'0000111 ??
    //alt_u8 = unsigned 8bit int - of which the first 4 bits are always 0 thanks to the line above. |val = 0000xxx|
    * led = (8 >> val) | (8 << (8 - val)); //the led "index" = b'1000 shifted by val (val <=7) OR'd with b'1000 left shifted by 8-val?
// val =<7 . the LHS = 0 when val >=4. If val >=4, the RHS = 8<<(8-(val>=4)) = 8 << x where x <= 4
// So - non zero LHS = 8>>(x<4) ; non zero RHS  = 8<<(x<4)
// when x = 3, 8>>3 | 8<<3 = 0001 | 1000 = 1001        9
// when x = 2, 8>>2 | 8<<2 = 0010 | 0100 = 0110        6
// when x = 1, 8>>1 | 8<<1 = 0100 | 0010 = 0110        6
// when x = 0, 8>>0 | 8<<0 = 1000 | 0001 = 1001        9
    * level = (acc_read >> 1) & 0x1f;
    // global int level is = acc_read shifted right by 1 and ANDED with 0x1f = b'00011111, hence level = b'000xxxxx where the xxxxx are from acc read
    //                                                                                                  acc read = 16 bit, xxxxx = bits [5:1] because
    //                                                                                                                      it was right shifted by 1
}
//convert_read(x_read, & level, & led);
//x read is accel data, level is some global int, led is the led we want
//x-read is a 16 bit 2's complement signed value for the x-axis of the G-Sensor

void sys_timer_isr() {
    IOWR_ALTERA_AVALON_TIMER_STATUS(TIMER_BASE, 0);

    if (pwm < abs(level)) {
    //pwm is defined as 0
    // hence pwm is always smaller than abs(level) unless level = 0 
    // pwm increases, so after a certain amount of cycles we get pwm to be big enough that we can start using the leds properly?

        if (level < 0) {
            //if level is negative, turn on (led<<1) which can equal:
            //1001 << 1 = 0010 = 2
            //0110 << 1 = 1100 = 10???
            led_write(led << 1);
        } else {
            led_write(led >> 1);
            //if level is positive turn on (led>>1) which can equal:
            //1001 >> 1 = 0100 = 4
            //0110 >> 1 = 0011 = 3
        }

    } else {
        led_write(led);
        //write the led with led which is only 6 or 9?
    }

    if (pwm > PWM_PERIOD) {
        pwm = 0;
    } else {
        pwm++;
        // pwm increases, so after a certain amount of cycles we get pwm to be big enough that we can start using the leds properly?
    }

}

void timer_init(void * isr) {

    IOWR_ALTERA_AVALON_TIMER_CONTROL(TIMER_BASE, 0x0003);
    IOWR_ALTERA_AVALON_TIMER_STATUS(TIMER_BASE, 0);
    IOWR_ALTERA_AVALON_TIMER_PERIODL(TIMER_BASE, 0x0900);
    IOWR_ALTERA_AVALON_TIMER_PERIODH(TIMER_BASE, 0x0000);
    alt_irq_register(TIMER_IRQ, 0, isr);
    IOWR_ALTERA_AVALON_TIMER_CONTROL(TIMER_BASE, 0x0007);

}

int main() {

    alt_32 x_read;
    alt_up_accelerometer_spi_dev * acc_dev;
    acc_dev = alt_up_accelerometer_spi_open_dev("/dev/accelerometer_spi");
    if (acc_dev == NULL) { // if return 1, check if the spi ip name is "accelerometer_spi"
        return 1;
    }

    timer_init(sys_timer_isr);
    while (1) {

        alt_up_accelerometer_spi_read_x_axis(acc_dev, & x_read);
        // alt_printf("raw data: %x\n", x_read);
        convert_read(x_read, & level, & led);

    }

    return 0;
}

