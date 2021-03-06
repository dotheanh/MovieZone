using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieZone.Models
{

    public class MovieRental
    {
        [Key]
        public int Id { get; set; }

        public int UserId { set; get; }
        [ForeignKey("UserId")]
        public User User { get; set; }       // người thuê phim

        public int MovieId { set; get; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }     // phim được thuê

        [DisplayName("Rent Date")]
        [DataType(DataType.Date)]
        public DateTime RentDate { get; set; }  // Ngày bắt đầu thuê
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }   // Ngày kết thúc thuê

        public TimeSpan Duration { get; set; }   // TimeSpan là một struct để đại diện cho khoảng cách giữa 2 DateTime
        [DisplayName("Total Price")]
        public decimal TotalPrice { get; set; }	// Tổng tiền

        // public MovieRental(User User, Movie Movie, DateTime RentDate, DateTime EndDate)    // tạo bằng ngày kết thúc thuê
        // {
        //     this.User = User;
        //     this.Movie = Movie;
        //     this.RentDate = RentDate;
        //     this.EndDate = EndDate;
        //     this.Duration = (TimeSpan)(RentDate - EndDate);
        //     this.TotalPrice = Movie.Price * Duration.Days; // Duration.Days : chuyển Duration về dạng int số ngày
        // }

        // public MovieRental(User User, Movie Movie, DateTime RentDate, TimeSpan Duration)    // tạo bằng thời hạn thuê
        // {
        //     this.User = User;
        //     this.Movie = Movie;
        //     this.RentDate = RentDate;
        //     this.EndDate = RentDate + Duration;
        //     this.Duration = Duration;
        //     this.TotalPrice = Movie.Price * Duration.Days; // Duration.Days : chuyển Duration về dạng int số ngày
        // }

        // public MovieRental(){
        //     if (this.Duration != null)
        //         this.EndDate = this.RentDate + Duration;
        //     this.TotalPrice = this.Movie.Price * Duration.Days; // Duration.Days : chuyển Duration về dạng int số ngày
        // }

        public void ChangeDuration(TimeSpan NewDuration)
        {
            EndDate = EndDate + (NewDuration - Duration);
            Duration = NewDuration;
        }

    }
}
