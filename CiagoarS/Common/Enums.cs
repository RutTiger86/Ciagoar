namespace CiagoarS.Common.Enums
{
    public enum ErrorCode
    {
        EC_DB, //데이터베이스 에러
        EC_EX, // 미확인 익셉션
        RE_NEXIST_USER, // 사용자 정보 없음 
        RE_EXIST_USER_NEXIST_OAUTH, // 사용자 정보가 있으나 해당 3rdParty로는 등록 안됨 
        RE_OAUTH_REFRASH_TOKKEN_EXPIRED, // Refrash Tokken 유효기간 만료 
        RE_OAUTH_UNSUPPORTED_3RDPARTY_LOGIN, // 지원하지 않는 3rdParty Login
    }
}
