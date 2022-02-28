#include "system.h"
#include "altera_up_avalon_accelerometer_spi.h"
#include "altera_avalon_timer_regs.h"
#include "altera_avalon_timer.h"
#include "altera_avalon_pio_regs.h"
#include "sys/alt_irq.h"
#include <stdlib.h>
#include <sys/alt_stdio.h>

#include "alt_types.h"
#include "sys/times.h"

#define OFFSET -32
#define PWM_PERIOD 16
#define N 10

alt_8 pwm = 0;
alt_u8 led;
int level;

alt_32 avg[N];

alt_32 movingAVG(alt_32 readVal){

	alt_32 average = 0;

	for (int i = 1; i < N; i++){
		avg[i-1] = avg[i];
		average += (1.0/N)*avg[i-1];
	}
	avg[N-1] = readVal;
	average += (1.0/N)*avg[N-1];

	return average;

}

alt_32 movingAVG2(alt_32 readVal){

	alt_32 average = 0;

	for (int i = 1; i < N; i++){
		avg[i-1] = avg[i];
		average += avg[i-1];
	}
	avg[N-1] = readVal;
	average += avg[N-1];
	return average/N;
}

void led_write(alt_u8 led_pattern) {
	IOWR(LED_BASE, 0, led_pattern);
}

// led_pattern = 10 bits, each corresponds to a value

void convert_read(alt_32 acc_read, int * level, alt_u8 * led) {

	alt_32 acc = movingAVG(acc_read);

	alt_printf("VALUE: %x ", acc);

	alt_u8 val = (acc >> 6) & 0x07; // 00000111
	* led = (8 >> val) | (8 << (8 - val));
	* level = (acc >> 1) & 0x1f; // 00011111
}



void sys_timer_isr() {
	IOWR_ALTERA_AVALON_TIMER_STATUS(TIMER_BASE, 0);

	if (pwm < abs(level)) {

		if (level < 0) {
			led_write(led << 1);
		} else {
			led_write(led >> 1);
		}
	} else {
		led_write(led);
	}

	if (pwm > PWM_PERIOD) {
		pwm = 0;
	} else {
		pwm++;
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
	alt_32 y_read;
	alt_32 z_read;
	alt_up_accelerometer_spi_dev * acc_dev;
	acc_dev = alt_up_accelerometer_spi_open_dev("/dev/accelerometer_spi");
	if (acc_dev == NULL) { // if return 1, check if the spi ip name is "accelerometer_spi"
	return 1;
}

	timer_init(sys_timer_isr);

	while (1) {
		alt_up_accelerometer_spi_read_x_axis(acc_dev, & x_read);
		alt_up_accelerometer_spi_read_y_axis(acc_dev, & y_read);
		alt_up_accelerometer_spi_read_z_axis(acc_dev, & z_read);
		//alt_printf("X AXIS = %x || Y AXIS = %x || Z AXIS = %x \n",movingAVG2(x_read),movingAVG2(y_read),movingAVG2(z_read));
		alt_printf("%x\n",x_read);
		alt_printf("%x\n",y_read);
		alt_printf("%x\n",movingAVG(z_read));
		//printf("%i \n",movingAVG(x_read));

		for(int i=0;i++;i<100000){
		}
	}

	return 0;
}
