<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class DanhGia extends Model
{
    protected $table = 'danhgia'; // Tên bảng trong cơ sở dữ liệu

    protected $fillable = [
        'user_id', 
        'sach_id',
        'vanphongpham_id',
        'diemdanhgia', 
        'nhanxet',
    ];

    public function user()
    {
        return $this->belongsTo(User::class);
    }

    public function sach()
    {
        return $this->belongsTo(Sach::class, 'sach_id', 'id');
    }

    public function vanPhongPham()
    {
        return $this->belongsTo(VanPhongPham::class, 'vpp_id', 'id');
    }

}
