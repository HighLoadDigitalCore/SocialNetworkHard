using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using sexivirt.Model;
using sexivirt.Web.Models.ViewModels;
using sexivirt.Web.Models.ViewModels.User;
using Tool;
namespace sexivirt.Web.Models.Mappers
{
    public static class MapperCollection
    {
        public static class LoginUserMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<User, LoginView>();
                Mapper.CreateMap<LoginView, User>();
            }
        }

        public static class UserMapper
        {
            public static void Init()
            {
                
                Mapper.CreateMap<User, RegisterUserView>()
                    .ForMember(dest => dest.BirthdateDay, opt => opt.MapFrom(src => src.Birthday.HasValue ? src.Birthday.Value.Day : 1))
                    .ForMember(dest => dest.BirthdateMonth, opt => opt.MapFrom(src => src.Birthday.HasValue ? src.Birthday.Value.Month : 1))
                    .ForMember(dest => dest.BirthdateYear, opt => opt.MapFrom(src => src.Birthday.HasValue ? src.Birthday.Value.Year : 1970));
                Mapper.CreateMap<RegisterUserView, User>()
                    .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => new DateTime(src.BirthdateYear, src.BirthdateMonth, src.BirthdateDay)));
                Mapper.CreateMap<User, UserView>()
                    .ForMember(x => x.StartIP, opt => opt.MapFrom(src => src.StartIP.ToIPString()))
                    .ForMember(x => x.EndIP, opt => opt.MapFrom(src => src.EndIP.ToIPString()));
                Mapper.CreateMap<UserView, User>()
                    .ForMember(x => x.StartIP, opt => opt.MapFrom(src => src.StartIP.ToIPInt()))
                    .ForMember(x => x.EndIP, opt => opt.MapFrom(src => src.EndIP.ToIPInt()));
                Mapper.CreateMap<VKSignUpViewModel, User>();

                Mapper.CreateMap<User, UserInfoView>()
                    .ForMember(dest => dest.BirthdateDay, opt => opt.MapFrom(src => src.Birthday.HasValue ? src.Birthday.Value.Day : 1))
                    .ForMember(dest => dest.BirthdateMonth, opt => opt.MapFrom(src => src.Birthday.HasValue ? src.Birthday.Value.Month : 1))
                    .ForMember(dest => dest.BirthdateYear, opt => opt.MapFrom(src => src.Birthday.HasValue ? src.Birthday.Value.Year : 1970));
                Mapper.CreateMap<UserInfoView, User>()
                    .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => new DateTime(src.BirthdateYear, src.BirthdateMonth, src.BirthdateDay)));

                Mapper.CreateMap<User, UserDescriptionView>();
                Mapper.CreateMap<UserDescriptionView, User>();
            }
        }

        
        public static class AlbumMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Album, AlbumView>()
                    .ForMember(p => p.IsPayed, opt => opt.MapFrom(src=> src.Price > 0));
                Mapper.CreateMap<AlbumView, Album>()
                    .ForMember(p => p.Photos, opt => opt.Ignore());
        	}
        }

        
        public static class BlogPostMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<BlogPost, BlogPostView>();
        		Mapper.CreateMap<BlogPostView, BlogPost>();
        	}
        }

        
        public static class CityMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<City, CityView>();
        		Mapper.CreateMap<CityView, City>();
        	}
        }

        
        public static class CommentMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Comment, CommentView>();
        		Mapper.CreateMap<CommentView, Comment>();
        	}
        }

        
        public static class EventMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Event, EventView>();
        		Mapper.CreateMap<EventView, Event>();
        	}
        }

        
        public static class GiftMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Gift, GiftView>();
        		Mapper.CreateMap<GiftView, Gift>();
        	}
        }

        
        public static class GroupMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Group, GroupView>();
        		Mapper.CreateMap<GroupView, Group>();
        	}
        }

        
        public static class GroupBlogPostMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<GroupBlogPost, GroupBlogPostView>();
        		Mapper.CreateMap<GroupBlogPostView, GroupBlogPost>();
        	}
        }

        
        public static class MeetingMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Meeting, MeetingView>();
        		Mapper.CreateMap<MeetingView, Meeting>();
        	}
        }

        
        public static class MessageMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Message, MessageView>();
        		Mapper.CreateMap<MessageView, Message>();
        	}
        }

        
        public static class PreferenceMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<Preference, PreferenceView>();
        		Mapper.CreateMap<PreferenceView, Preference>();
        	}
        }

        public static class PhotoMapper
        {
            public static void Init()
            {
                Mapper.CreateMap<Photo, PhotoView>();
                Mapper.CreateMap<PhotoView, Photo>();
            }
        }

        
        public static class DepositCandidateMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<DepositCandidate, DepositCandidateView>();
        		Mapper.CreateMap<DepositCandidateView, DepositCandidate>();
        	}
        }

        
        public static class MoneyWithdrawMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<MoneyWithdraw, MoneyWithdrawView>();
        		Mapper.CreateMap<MoneyWithdrawView, MoneyWithdraw>();
        	}
        }

        
        public static class UserGiftMapper
        {
        	public static void Init()
        	{
        		Mapper.CreateMap<UserGift, UserGiftView>();
        		Mapper.CreateMap<UserGiftView, UserGift>();
        	}
        }
    }
}