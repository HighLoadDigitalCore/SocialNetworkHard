using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using sexivirt.Model;
using sexivirt.Web.Models.ViewModels;
using sexivirt.Web.Models.Mappers;


namespace sexivirt.Web.Mappers
{
    public class CommonMapper 
    {
        static CommonMapper()
        {
            MapperCollection.LoginUserMapper.Init();
            MapperCollection.UserMapper.Init();
            MapperCollection.AlbumMapper.Init();
            MapperCollection.BlogPostMapper.Init();
            MapperCollection.CityMapper.Init();
            MapperCollection.CommentMapper.Init();
            MapperCollection.EventMapper.Init();
            MapperCollection.GiftMapper.Init();
            MapperCollection.GroupMapper.Init();
            MapperCollection.GroupBlogPostMapper.Init();
            MapperCollection.MeetingMapper.Init();
            MapperCollection.MessageMapper.Init();
            MapperCollection.PreferenceMapper.Init();
            MapperCollection.PhotoMapper.Init();
            MapperCollection.DepositCandidateMapper.Init();
            MapperCollection.MoneyWithdrawMapper.Init();
            MapperCollection.UserGiftMapper.Init();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}